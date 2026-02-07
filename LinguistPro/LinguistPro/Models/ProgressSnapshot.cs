using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Tracks learning progress snapshots for analytics and charting.
    /// Stores daily/weekly progress metrics for trend analysis.
    /// </summary>
    public class ProgressSnapshot
    {
        [Key]
        public int SnapshotId { get; set; }

        [Required]
        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// Date of this snapshot (in UTC)
        /// </summary>
        [Required]
        public DateTime SnapshotDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Snapshot period type
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PeriodType { get; set; } = "Daily"; // "Daily", "Weekly", "Monthly"

        /// <summary>
        /// Total items in learning system
        /// </summary>
        [Required]
        public int TotalItems { get; set; } = 0;

        /// <summary>
        /// Items added this period
        /// </summary>
        [Required]
        public int ItemsAdded { get; set; } = 0;

        /// <summary>
        /// Average mastery level (0-100)
        /// </summary>
        [Required]
        public double AverageMastery { get; set; } = 0;

        /// <summary>
        /// Items mastered (mastery >= 90%)
        /// </summary>
        [Required]
        public int ItemsMastered { get; set; } = 0;

        /// <summary>
        /// Total time spent learning (in seconds)
        /// </summary>
        [Required]
        public long TotalTimeSpentSeconds { get; set; } = 0;

        /// <summary>
        /// Learning streak at this point
        /// </summary>
        [Required]
        public int CurrentStreak { get; set; } = 0;

        /// <summary>
        /// Items reviewed this period
        /// </summary>
        [Required]
        public int ItemsReviewed { get; set; } = 0;

        /// <summary>
        /// Retention rate (0-100%)
        /// </summary>
        [Required]
        public double RetentionRate { get; set; } = 0;

        /// <summary>
        /// When this snapshot was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public LanguageProfile? LanguageProfile { get; set; }

        /// <summary>
        /// Get formatted time spent
        /// </summary>
        public string GetFormattedTimeSpent()
        {
            var span = TimeSpan.FromSeconds(TotalTimeSpentSeconds);
            if (span.TotalHours > 1)
                return $"{span.TotalHours:F1}h";
            if (span.TotalMinutes > 1)
                return $"{span.TotalMinutes:F0}m";
            return $"{span.TotalSeconds:F0}s";
        }

        public override string ToString()
        {
            return $"{SnapshotDate:yyyy-MM-dd} ({PeriodType}): {AverageMastery:F1}% mastery";
        }
    }
}
