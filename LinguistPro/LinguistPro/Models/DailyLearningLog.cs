using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Records daily learning activity for tracking streaks and consistency.
    /// One entry per language per day to track if the user learned that day.
    /// </summary>
    public class DailyLearningLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// The date of learning activity (in UTC, time portion is always midnight)
        /// </summary>
        [Required]
        public DateTime LearningDate { get; set; }

        /// <summary>
        /// Number of vocabulary items learned/reviewed this day
        /// </summary>
        [Required]
        public int VocabularyItemsLearned { get; set; } = 0;

        /// <summary>
        /// Number of verbs learned/reviewed this day
        /// </summary>
        [Required]
        public int VerbsLearned { get; set; } = 0;

        /// <summary>
        /// Number of language items (numbers, months, days) learned/reviewed this day
        /// </summary>
        [Required]
        public int LanguageItemsLearned { get; set; } = 0;

        /// <summary>
        /// Total learning time in seconds for this day
        /// </summary>
        [Required]
        public int TotalTimeSpentSeconds { get; set; } = 0;

        /// <summary>
        /// Number of items mastered (reached 100%) on this day
        /// </summary>
        [Required]
        public int ItemsMastered { get; set; } = 0;

        /// <summary>
        /// Overall mastery level at end of day
        /// </summary>
        [Required]
        public int MasteryLevelAtEndOfDay { get; set; } = 0;

        /// <summary>
        /// When this log entry was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When this log entry was last updated
        /// </summary>
        [Required]
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public LanguageProfile? LanguageProfile { get; set; }

        /// <summary>
        /// Get total items learned on this day
        /// </summary>
        public int GetTotalItemsLearned()
        {
            return VocabularyItemsLearned + VerbsLearned + LanguageItemsLearned;
        }

        /// <summary>
        /// Get total time spent formatted as HH:mm:ss
        /// </summary>
        public string GetFormattedTimeSpent()
        {
            var span = TimeSpan.FromSeconds(TotalTimeSpentSeconds);
            return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
        }

        public override string ToString()
        {
            return $"Learning Log: {LearningDate:yyyy-MM-dd} - {GetTotalItemsLearned()} items";
        }
    }
}
