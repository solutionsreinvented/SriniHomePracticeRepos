using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for managing user preferences and settings
    /// </summary>
    public class UserPreferencesService
    {
        private readonly AppDbContext _context;

        public UserPreferencesService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get or create user preferences
        /// </summary>
        public async Task<UserPreferences> GetOrCreatePreferencesAsync(int userId)
        {
            var preferences = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences != null)
                return preferences;

            // Create default preferences for new user
            preferences = new UserPreferences
            {
                UserId = userId,
                Theme = "light",
                FontSize = "medium",
                ItemsPerPage = 20,
                EnableEmailNotifications = true,
                EnablePushNotifications = true,
                EnableStreakReminders = true,
                DailyReminderHour = 9,
                EnableSoundNotifications = true,
                AutoSaveInterval = 30,
                ShowPronunciationGuide = true,
                ShowUsageExamples = true,
                UILanguage = "en",
                LastUpdated = DateTime.UtcNow
            };

            _context.UserPreferences.Add(preferences);
            await _context.SaveChangesAsync();

            return preferences;
        }

        /// <summary>
        /// Update user preferences
        /// </summary>
        public async Task<UserPreferences> UpdatePreferencesAsync(UserPreferences preferences)
        {
            preferences.LastUpdated = DateTime.UtcNow;
            _context.UserPreferences.Update(preferences);
            await _context.SaveChangesAsync();
            return preferences;
        }

        /// <summary>
        /// Update individual preference property
        /// </summary>
        public async Task UpdatePreferenceAsync(int userId, string propertyName, object value)
        {
            var preferences = await GetOrCreatePreferencesAsync(userId);

            var property = typeof(UserPreferences).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(preferences, value);
                preferences.LastUpdated = DateTime.UtcNow;
                _context.UserPreferences.Update(preferences);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' does not exist or is not writable");
            }
        }

        /// <summary>
        /// Delete user preferences (when user is deleted)
        /// </summary>
        public async Task DeletePreferencesAsync(int userId)
        {
            var preferences = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences != null)
            {
                _context.UserPreferences.Remove(preferences);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Reset preferences to defaults
        /// </summary>
        public async Task ResetToDefaultsAsync(int userId)
        {
            var preferences = await GetOrCreatePreferencesAsync(userId);

            preferences.Theme = "light";
            preferences.FontSize = "medium";
            preferences.ItemsPerPage = 20;
            preferences.EnableEmailNotifications = true;
            preferences.EnablePushNotifications = true;
            preferences.EnableStreakReminders = true;
            preferences.DailyReminderHour = 9;
            preferences.EnableSoundNotifications = true;
            preferences.AutoSaveInterval = 30;
            preferences.ShowPronunciationGuide = true;
            preferences.ShowUsageExamples = true;
            preferences.UILanguage = "en";
            preferences.LastUpdated = DateTime.UtcNow;

            _context.UserPreferences.Update(preferences);
            await _context.SaveChangesAsync();
        }
    }
}
