using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinguistPro.Models;

namespace LinguistPro.Pages.Account
{
    [Authorize]
    public class ManageLanguagesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public ManageLanguagesModel(UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public List<LanguageProfileViewModel> UserLanguages { get; set; } = new();
        public List<LanguageProfileViewModel> AvailableLanguages { get; set; } = new();

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedLanguageCode { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            // Get user's language profiles
            UserLanguages = await _db.LanguageProfiles
                .Where(lp => lp.UserId == user.Id)
                .OrderBy(lp => lp.LanguageName)
                .Select(lp => new LanguageProfileViewModel
                {
                    LanguageProfileId = lp.LanguageProfileId,
                    LanguageCode = lp.LanguageCode,
                    LanguageName = lp.LanguageName,
                    IsActive = lp.IsActive,
                    MasteryLevel = lp.MasteryLevel,
                    ItemCount = _db.Vocabulary.Count(v => v.LanguageProfileId == lp.LanguageProfileId) +
                               _db.Verbs.Count(v => v.LanguageProfileId == lp.LanguageProfileId) +
                               _db.LanguageItems.Count(i => i.LanguageProfileId == lp.LanguageProfileId)
                })
                .ToListAsync();

            // Get available languages that user doesn't have
            var userLanguageCodes = UserLanguages.Select(ul => ul.LanguageCode).ToList();
            AvailableLanguages = Pages.IndexModel.AvailableLanguages
                .Where(al => !userLanguageCodes.Contains(al.Key))
                .Select(al => new LanguageProfileViewModel
                {
                    LanguageCode = al.Key,
                    LanguageName = al.Value
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAddLanguageAsync()
        {
            if (string.IsNullOrEmpty(SelectedLanguageCode))
            {
                ErrorMessage = "Please select a language.";
                await OnGetAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            // Check if language already exists
            var existingProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(lp => lp.UserId == user.Id && lp.LanguageCode == SelectedLanguageCode);

            if (existingProfile != null)
            {
                ErrorMessage = "You already have this language added.";
                await OnGetAsync();
                return Page();
            }

            if (!Pages.IndexModel.AvailableLanguages.ContainsKey(SelectedLanguageCode))
            {
                ErrorMessage = "Invalid language selected.";
                await OnGetAsync();
                return Page();
            }

            try
            {
                var languageName = Pages.IndexModel.AvailableLanguages[SelectedLanguageCode];
                var newProfile = new LanguageProfile
                {
                    UserId = user.Id,
                    LanguageCode = SelectedLanguageCode,
                    LanguageName = languageName,
                    IsActive = false,
                    CreatedDate = DateTime.UtcNow,
                    MasteryLevel = 0
                };

                _db.LanguageProfiles.Add(newProfile);
                await _db.SaveChangesAsync();

                SuccessMessage = $"✓ {languageName} has been added to your languages!";
                SelectedLanguageCode = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error adding language: {ex.Message}";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLanguageAsync(int languageProfileId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(lp => lp.LanguageProfileId == languageProfileId && lp.UserId == user.Id);

            if (langProfile == null)
            {
                ErrorMessage = "Language profile not found.";
                await OnGetAsync();
                return Page();
            }

            if (UserLanguages.Count <= 1)
            {
                ErrorMessage = "You must keep at least one language.";
                await OnGetAsync();
                return Page();
            }

            try
            {
                var languageName = langProfile.LanguageName;

                // Delete all related data
                var vocabItems = _db.Vocabulary.Where(v => v.LanguageProfileId == languageProfileId);
                var verbItems = _db.Verbs.Where(v => v.LanguageProfileId == languageProfileId);
                var languageItems = _db.LanguageItems.Where(i => i.LanguageProfileId == languageProfileId);

                _db.Vocabulary.RemoveRange(vocabItems);
                _db.Verbs.RemoveRange(verbItems);
                _db.LanguageItems.RemoveRange(languageItems);
                _db.LanguageProfiles.Remove(langProfile);

                await _db.SaveChangesAsync();

                SuccessMessage = $"✓ {languageName} and all its data have been removed.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error removing language: {ex.Message}";
            }

            await OnGetAsync();
            return Page();
        }
    }

    public class LanguageProfileViewModel
    {
        public int LanguageProfileId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MasteryLevel { get; set; }
        public int ItemCount { get; set; }
    }
}
