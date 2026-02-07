using LinguistPro.Models;
using LinguistPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Pages
{
    [Authorize]
    public class VocabularyWithPronunciationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly PronunciationFetcherService _pronunciationFetcher;
        private readonly ILogger<VocabularyWithPronunciationModel> _logger;

        public VocabularyWithPronunciationModel(
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            PronunciationFetcherService pronunciationFetcher,
            ILogger<VocabularyWithPronunciationModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _pronunciationFetcher = pronunciationFetcher;
            _logger = logger;
        }

        public List<VocabularyPronunciationViewModel> VocabularyWithPronunciation { get; set; } = [];
        public string? SelectedLanguage { get; set; }
        public List<string> AvailableLanguages { get; set; } = [];
        public int VocabularyCount { get; set; }

        public async Task OnGetAsync(string? selectedLanguage = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return;

            SelectedLanguage = selectedLanguage;

            // Get user's language profiles
            var languageProfiles = await _context.LanguageProfiles
                .Where(l => l.UserId == user.Id)
                .Include(l => l.VocabularyItems)
                .ToListAsync();

            // Build available languages list
            AvailableLanguages = languageProfiles
                .Select(l => l.LanguageCode)
                .Distinct()
                .ToList();

            // Get vocabulary with real-time pronunciation fetching
            var query = _context.Vocabulary
                .Include(v => v.LanguageProfile)
                .Where(v => v.LanguageProfile.UserId == user.Id);

            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                query = query.Where(v => v.LanguageProfile.LanguageCode == selectedLanguage);
            }

            var vocabularyItems = await query
                .OrderBy(v => v.Term)
                .Take(100)  // Limit to avoid too many API calls
                .ToListAsync();

            _logger.LogInformation($"Fetching pronunciation for {vocabularyItems.Count} vocabulary items");

            // Fetch pronunciation in real-time for each vocabulary item
            VocabularyWithPronunciation = new List<VocabularyPronunciationViewModel>();

            foreach (var vocab in vocabularyItems)
            {
                try
                {
                    // Try to get from cache first
                    PronunciationData? pronunciation = await _context.PronunciationData
                        .FirstOrDefaultAsync(p => p.Word.ToLower() == vocab.Term.ToLower() 
                            && p.LanguageCode == vocab.LanguageProfile.LanguageCode);

                    // If not in cache, fetch from API
                    if (pronunciation == null)
                    {
                        _logger.LogInformation($"Fetching pronunciation for: {vocab.Term}");
                        pronunciation = await _pronunciationFetcher.FetchComprehensiveAsync(
                            vocab.Term, 
                            vocab.LanguageProfile.LanguageCode);

                        // Cache the result
                        if (pronunciation != null)
                        {
                            _context.PronunciationData.Add(pronunciation);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"✓ Cached pronunciation for: {vocab.Term}");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"✓ Found cached pronunciation for: {vocab.Term}");
                    }

                    VocabularyWithPronunciation.Add(new VocabularyPronunciationViewModel
                    {
                        Vocabulary = vocab,
                        Pronunciation = pronunciation
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error fetching pronunciation for {vocab.Term}: {ex.Message}");
                    VocabularyWithPronunciation.Add(new VocabularyPronunciationViewModel
                    {
                        Vocabulary = vocab,
                        Pronunciation = null
                    });
                }
            }

            VocabularyCount = VocabularyWithPronunciation.Count;

            _logger.LogInformation($"Loaded {VocabularyCount} vocabulary items with pronunciation");
        }

        public string GetLanguageBadge(string languageCode)
        {
            return languageCode switch
            {
                "de" => "🇩🇪 German",
                "fr" => "🇫🇷 French",
                "es" => "🇪🇸 Spanish",
                "ru" => "🇷🇺 Russian",
                "ko" => "🇰🇷 Korean",
                _ => languageCode.ToUpper()
            };
        }
    }

    /// <summary>
    /// View model combining vocabulary with pronunciation data
    /// </summary>
    public class VocabularyPronunciationViewModel
    {
        public VocabularyItem Vocabulary { get; set; } = null!;
        public PronunciationData? Pronunciation { get; set; }
    }
}
