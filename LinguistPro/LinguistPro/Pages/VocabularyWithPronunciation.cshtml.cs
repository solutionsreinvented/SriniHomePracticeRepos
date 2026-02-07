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
        private readonly PronunciationService _pronunciationService;

        public VocabularyWithPronunciationModel(
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            PronunciationService pronunciationService)
        {
            _userManager = userManager;
            _context = context;
            _pronunciationService = pronunciationService;
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

            // Get vocabulary with pronunciation
            var query = _context.Vocabulary
                .Include(v => v.LanguageProfile)
                .Where(v => v.LanguageProfile.UserId == user.Id);

            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                query = query.Where(v => v.LanguageProfile.LanguageCode == selectedLanguage);
            }

            var vocabularyItems = await query.ToListAsync();

            // Build view models with pronunciation data
            VocabularyWithPronunciation = new List<VocabularyPronunciationViewModel>();

            foreach (var vocab in vocabularyItems)
            {
                var pronunciation = await _pronunciationService
                    .GetPronunciationAsync(vocab.Term, vocab.LanguageProfile.LanguageCode);

                VocabularyWithPronunciation.Add(new VocabularyPronunciationViewModel
                {
                    Vocabulary = vocab,
                    Pronunciation = pronunciation
                });
            }

            // Sort by whether pronunciation exists (items with pronunciation first)
            VocabularyWithPronunciation = VocabularyWithPronunciation
                .OrderByDescending(v => v.Pronunciation != null)
                .ThenBy(v => v.Vocabulary.Term)
                .ToList();

            VocabularyCount = VocabularyWithPronunciation.Count;
        }

        public string GetLanguageName(string languageCode)
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
