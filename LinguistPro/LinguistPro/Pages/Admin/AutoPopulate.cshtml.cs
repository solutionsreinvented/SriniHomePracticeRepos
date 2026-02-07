using LinguistPro.Models;
using LinguistPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Pages.Admin
{
    [Authorize]
    public class AutoPopulateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly LanguageDataAutoPopulatorService _autoPopulator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AutoPopulateModel> _logger;

        public Dictionary<string, string> AvailableLanguages { get; set; } = new();
        public List<string> Categories { get; set; } = new() { "Vocabulary", "Verbs", "Numbers", "Days", "Months" };

        public bool IsSuccess { get; set; }
        public bool HasError { get; set; }
        public bool IsProcessing { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ProcessingMessage { get; set; }
        public int VocabularyCount { get; set; }
        public int VerbCount { get; set; }
        public int ProcessedCount { get; set; }
        public int TotalCount { get; set; }
        public double ProgressPercentage { get; set; }

        public AutoPopulateModel(
            AppDbContext context,
            LanguageDataAutoPopulatorService autoPopulator,
            UserManager<ApplicationUser> userManager,
            ILogger<AutoPopulateModel> logger)
        {
            _context = context;
            _autoPopulator = autoPopulator;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            // Get user's language profiles
            var languages = await _context.LanguageProfiles
                .Where(l => l.UserId == user.Id)
                .Select(l => l.LanguageCode)
                .Distinct()
                .ToListAsync();

            AvailableLanguages = languages.ToDictionary(
                lang => lang,
                lang => GetLanguageName(lang)
            );
        }

        public async Task<IActionResult> OnPostAsync(
            string languageCode,
            string category = "Vocabulary",
            int vocabularyCount = 100,
            int verbCount = 100)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    HasError = true;
                    ErrorMessage = "User not found";
                    return Page();
                }

                IsProcessing = true;

                // Get user's language profile
                var languageProfile = await _context.LanguageProfiles
                    .FirstOrDefaultAsync(l => l.UserId == user.Id && l.LanguageCode == languageCode);

                if (languageProfile == null)
                {
                    HasError = true;
                    ErrorMessage = $"Language profile not found for {GetLanguageName(languageCode)}";
                    return Page();
                }

                // Create progress tracker
                var progressReporter = new Progress<LanguageDataAutoPopulatorService.ProgressEventArgs>(args =>
                {
                    ProcessedCount = args.ProcessedCount;
                    TotalCount = args.TotalCount;
                    ProcessingMessage = args.Status;
                    if (args.TotalCount > 0)
                    {
                        ProgressPercentage = (double)args.ProcessedCount / args.TotalCount * 100;
                    }
                    _logger.LogInformation($"Progress: {args.ProcessedCount}/{args.TotalCount} - {args.CurrentItem}");
                });

                _logger.LogInformation($"Starting auto-population: {category} for {languageCode}");

                // Handle different categories
                if (category == "Vocabulary")
                {
                    var vocabularyItems = await _autoPopulator.AutoPopulateVocabularyAsync(
                        languageProfile.LanguageProfileId,
                        languageCode,
                        vocabularyCount,
                        progressReporter);

                    VocabularyCount = vocabularyItems.Count;
                    SuccessMessage = $"Successfully populated {VocabularyCount} vocabulary items with real definitions and examples!";
                }
                else if (category == "Verbs")
                {
                    var verbEntries = await _autoPopulator.AutoPopulateVerbsAsync(
                        languageProfile.LanguageProfileId,
                        languageCode,
                        verbCount,
                        progressReporter);

                    VerbCount = verbEntries.Count;
                    SuccessMessage = $"Successfully populated {VerbCount} verb entries with proper conjugations!";
                }
                else if (category == "Numbers")
                {
                    var numbers = await _autoPopulator.AutoPopulateSpecialCategoriesAsync(
                        languageProfile.LanguageProfileId,
                        languageCode,
                        "numbers",
                        progressReporter);

                    SuccessMessage = $"Successfully populated {numbers.Count} numbers!";
                }
                else if (category == "Days")
                {
                    var days = await _autoPopulator.AutoPopulateSpecialCategoriesAsync(
                        languageProfile.LanguageProfileId,
                        languageCode,
                        "days",
                        progressReporter);

                    SuccessMessage = $"Successfully populated {days.Count} days!";
                }
                else if (category == "Months")
                {
                    var months = await _autoPopulator.AutoPopulateSpecialCategoriesAsync(
                        languageProfile.LanguageProfileId,
                        languageCode,
                        "months",
                        progressReporter);

                    SuccessMessage = $"Successfully populated {months.Count} months!";
                }

                IsSuccess = true;
                IsProcessing = false;
                ProgressPercentage = 100;

                // Reload available languages
                await OnGetAsync();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during auto-population: {ex.Message}");
                HasError = true;
                ErrorMessage = $"An error occurred: {ex.Message}";
                IsProcessing = false;
                return Page();
            }
        }

        private string GetLanguageName(string code)
        {
            return code switch
            {
                "de" => "German",
                "fr" => "French",
                "es" => "Spanish",
                "ru" => "Russian",
                "ko" => "Korean",
                _ => code.ToUpper()
            };
        }
    }
}
