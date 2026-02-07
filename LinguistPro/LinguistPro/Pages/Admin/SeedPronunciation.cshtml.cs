using LinguistPro.Data;
using LinguistPro.Models;
using LinguistPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinguistPro.Pages.Admin
{
    [Authorize]
    public class SeedPronunciationModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly PronunciationFetcherService _fetcher;
        private readonly ILogger<SeedPronunciationModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public bool IsSeeding { get; set; }
        public bool IsComplete { get; set; }
        public bool HasError { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Status { get; set; }

        public SeedPronunciationModel(
            AppDbContext context,
            PronunciationFetcherService fetcher,
            ILogger<SeedPronunciationModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fetcher = fetcher;
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet()
        {
            // Check if user is admin (optional - you can modify this)
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Check if already seeded
                if (_context.PronunciationData.Any())
                {
                    Message = "Pronunciation data already exists in database. Skipping seeding.";
                    IsComplete = true;
                    return Page();
                }

                IsSeeding = true;
                Status = "Initializing...";

                // Define words to seed
                var words = new Dictionary<string, List<string>>
                {
                    { "de", new List<string> { "hallo", "danke", "schön", "guten", "morgen", "nacht", "haus", "wasser", "brot", "käse" } },
                    { "fr", new List<string> { "bonjour", "merci", "oui", "non", "s'il vous plaît", "excusez", "amour", "eau", "pain", "fromage" } },
                    { "es", new List<string> { "hola", "gracias", "sí", "no", "por favor", "disculpe", "amor", "agua", "pan", "queso" } },
                    { "ru", new List<string> { "привет", "спасибо", "да", "нет", "пожалуйста", "извините", "любовь", "вода", "хлеб", "сыр" } },
                    { "ko", new List<string> { "안녕하세요", "감사합니다", "네", "아니오", "제발", "죄송합니다", "사랑", "물", "빵", "치즈" } }
                };

                int totalWords = words.Values.Sum(v => v.Count);
                int processedWords = 0;
                int successfulWords = 0;

                _logger.LogInformation($"Starting to seed {totalWords} pronunciation entries...");

                foreach (var language in words)
                {
                    var langCode = language.Key;
                    var wordList = language.Value;
                    var langName = GetLanguageName(langCode);

                    _logger.LogInformation($"Processing {langName}...");
                    Status = $"Processing {langName}...";

                    foreach (var word in wordList)
                    {
                        try
                        {
                            // Check if already exists
                            var existing = _context.PronunciationData
                                .FirstOrDefault(p => p.Word.ToLower() == word.ToLower() && p.LanguageCode == langCode);

                            if (existing != null)
                            {
                                processedWords++;
                                continue;
                            }

                            _logger.LogInformation($"Fetching: {word} ({langCode})");

                            // Fetch from free API
                            var pronunciation = await _fetcher.FetchComprehensiveAsync(word, langCode);

                            if (pronunciation != null)
                            {
                                // Add syllable breakdown if empty
                                if (string.IsNullOrEmpty(pronunciation.SyllableBreakdown))
                                {
                                    pronunciation.SyllableBreakdown = _fetcher.ParseSyllablesFromWord(word, langCode);
                                }

                                _context.PronunciationData.Add(pronunciation);
                                successfulWords++;
                                _logger.LogInformation($"✓ Added: {word} - {pronunciation.IPA}");
                            }
                            else
                            {
                                _logger.LogWarning($"✗ Failed to fetch: {word}");
                            }

                            processedWords++;
                            Status = $"Processing {langName}... ({processedWords}/{totalWords})";

                            // Small delay to avoid rate limiting
                            await Task.Delay(300);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error processing {word}: {ex.Message}");
                            processedWords++;
                        }
                    }
                }

                await _context.SaveChangesAsync();

                Message = $"✅ Successfully seeded {successfulWords} pronunciation entries from {processedWords} attempts!";
                IsComplete = true;
                IsSeeding = false;

                _logger.LogInformation($"Seeding completed! Added {successfulWords} entries.");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during seeding: {ex.Message}");
                HasError = true;
                ErrorMessage = $"An error occurred: {ex.Message}";
                IsSeeding = false;
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
