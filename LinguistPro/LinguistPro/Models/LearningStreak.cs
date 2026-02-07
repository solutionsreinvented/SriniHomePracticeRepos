using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Represents a user's learning streak for a specific language.
    /// Tracks consecutive days of learning to encourage consistency and build habits.
    /// </summary>
    public class LearningStreak
    {
        [Key]
        public int StreakId { get; set; }

        [Required]
        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// Current consecutive learning days
        /// </summary>
        [Required]
        public int CurrentStreak { get; set; } = 0;

        /// <summary>
        /// Longest streak ever achieved for this language
        /// </summary>
        [Required]
        public int LongestStreak { get; set; } = 0;

        /// <summary>
        /// Date when the current streak started
        /// </summary>
        [Required]
        public DateTime StreakStartDate { get; set; }

        /// <summary>
        /// Last date the user learned this language
        /// Used to determine if streak continues or resets
        /// </summary>
        [Required]
        public DateTime LastLearningDate { get; set; }

        /// <summary>
        /// Total number of days with learning activity (not consecutive)
        /// </summary>
        [Required]
        public int TotalLearningDays { get; set; } = 0;

        /// <summary>
        /// When this streak record was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When this streak record was last updated
        /// </summary>
        [Required]
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public LanguageProfile? LanguageProfile { get; set; }

        /// <summary>
        /// Get the milestone badge based on current streak
        /// </summary>
        public string GetStreakBadge()
        {
            return CurrentStreak switch
            {
                >= 100 => "🔥🔥🔥", // 100+ days badge
                >= 30 => "🔥🔥",     // 30+ days badge
                >= 7 => "🔥",        // 7+ days badge
                _ => ""              // No badge yet
            };
        }

        /// <summary>
        /// Check if streak is still active (learned yesterday or today)
        /// </summary>
        public bool IsStreakActive()
        {
            var today = DateTime.UtcNow.Date;
            var lastLearningDateOnly = LastLearningDate.Date;
            
            // Streak is active if last learning was today or yesterday
            return (today - lastLearningDateOnly).TotalDays <= 1;
        }

        public override string ToString()
        {
            return $"Streak: {CurrentStreak} days (Longest: {LongestStreak})";
        }
    }
}
