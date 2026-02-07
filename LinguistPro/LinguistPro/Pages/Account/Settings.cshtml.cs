using LinguistPro.Models;
using LinguistPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinguistPro.Pages.Account
{
    [Authorize]
    public class SettingsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserPreferencesService _preferencesService;

        public SettingsModel(UserManager<ApplicationUser> userManager, UserPreferencesService preferencesService)
        {
            _userManager = userManager;
            _preferencesService = preferencesService;
        }

        public UserPreferences? Preferences { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Redirect("/Account/Login");

            Preferences = await _preferencesService.GetOrCreatePreferencesAsync(user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UserPreferences model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Redirect("/Account/Login");

            try
            {
                var preferences = await _preferencesService.GetOrCreatePreferencesAsync(user.Id);
                
                // Update preferences from model
                preferences.Theme = model.Theme;
                preferences.FontSize = model.FontSize;
                preferences.DefaultLanguageCode = model.DefaultLanguageCode;
                preferences.ItemsPerPage = model.ItemsPerPage;
                preferences.EnableEmailNotifications = model.EnableEmailNotifications;
                preferences.EnablePushNotifications = model.EnablePushNotifications;
                preferences.EnableStreakReminders = model.EnableStreakReminders;
                preferences.DailyReminderHour = model.DailyReminderHour;
                preferences.EnableSoundNotifications = model.EnableSoundNotifications;
                preferences.AutoSaveInterval = model.AutoSaveInterval;
                preferences.ShowPronunciationGuide = model.ShowPronunciationGuide;
                preferences.ShowUsageExamples = model.ShowUsageExamples;

                await _preferencesService.UpdatePreferencesAsync(preferences);
                
                SuccessMessage = "✅ Your preferences have been saved successfully!";
                Preferences = preferences;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"❌ Error saving preferences: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnGetResetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Redirect("/Account/Login");

            try
            {
                await _preferencesService.ResetToDefaultsAsync(user.Id);
                SuccessMessage = "✅ Preferences have been reset to defaults!";
                Preferences = await _preferencesService.GetOrCreatePreferencesAsync(user.Id);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"❌ Error resetting preferences: {ex.Message}";
                Preferences = await _preferencesService.GetOrCreatePreferencesAsync(user.Id);
                return Page();
            }
        }
    }
}
