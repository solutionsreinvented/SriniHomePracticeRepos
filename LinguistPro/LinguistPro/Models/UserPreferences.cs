using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// User preferences and settings for personalization
    /// </summary>
    public class UserPreferences
    {
        [Key]
        public int PreferencesId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// Theme preference: "light" or "dark"
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Theme { get; set; } = "light";

        /// <summary>
        /// Font size preference: "small", "medium", "large"
        /// </summary>
        [Required]
        [StringLength(10)]
        public string FontSize { get; set; } = "medium";

        /// <summary>
        /// Default language code for new sessions (e.g., "de", "fr", "es")
        /// </summary>
        [StringLength(10)]
        public string? DefaultLanguageCode { get; set; }

        /// <summary>
        /// Number of items to display per page in lists
        /// </summary>
        [Range(5, 100)]
        public int ItemsPerPage { get; set; } = 20;

        /// <summary>
        /// Enable email notifications for milestones
        /// </summary>
        public bool EnableEmailNotifications { get; set; } = true;

        /// <summary>
        /// Enable push notifications
        /// </summary>
        public bool EnablePushNotifications { get; set; } = true;

        /// <summary>
        /// Enable streak reminders
        /// </summary>
        public bool EnableStreakReminders { get; set; } = true;

        /// <summary>
        /// Send daily learning reminder at this hour (0-23)
        /// </summary>
        [Range(0, 23)]
        public int DailyReminderHour { get; set; } = 9; // 9 AM default

        /// <summary>
        /// Enable sound notifications
        /// </summary>
        public bool EnableSoundNotifications { get; set; } = true;

        /// <summary>
        /// Auto-save learning progress (in seconds between saves)
        /// </summary>
        [Range(10, 3600)]
        public int AutoSaveInterval { get; set; } = 30; // 30 seconds

        /// <summary>
        /// Show pronunciation guide
        /// </summary>
        public bool ShowPronunciationGuide { get; set; } = true;

        /// <summary>
        /// Show usage examples in vocabulary
        /// </summary>
        public bool ShowUsageExamples { get; set; } = true;

        /// <summary>
        /// Language for UI interface (currently always "en", reserved for future)
        /// </summary>
        [Required]
        [StringLength(5)]
        public string UILanguage { get; set; } = "en";

        /// <summary>
        /// Last updated date
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
