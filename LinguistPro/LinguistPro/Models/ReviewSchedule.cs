using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Represents review schedule for a vocabulary or language item using Leitner algorithm.
    /// Items are organized into boxes with increasing review intervals.
    /// </summary>
    public class ReviewSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        [ForeignKey(nameof(VocabularyItem))]
        public int? VocabularyItemId { get; set; }

        [ForeignKey(nameof(LanguageItem))]
        public int? LanguageItemId { get; set; }

        /// <summary>
        /// Leitner box (1-5):
        /// Box 1: Review after 1 day
        /// Box 2: Review after 3 days
        /// Box 3: Review after 7 days
        /// Box 4: Review after 14 days
        /// Box 5: Review after 30 days (mastered)
        /// </summary>
        [Required]
        public int LeitnerBox { get; set; } = 1;

        /// <summary>
        /// Number of times this item has been reviewed correctly
        /// </summary>
        [Required]
        public int CorrectCount { get; set; } = 0;

        /// <summary>
        /// Number of times this item has been reviewed incorrectly
        /// </summary>
        [Required]
        public int IncorrectCount { get; set; } = 0;

        /// <summary>
        /// When this item was last reviewed
        /// </summary>
        [Required]
        public DateTime LastReviewedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When this item should be reviewed next (calculated based on Leitner box)
        /// </summary>
        [Required]
        public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date item was added to the learning system
        /// </summary>
        [Required]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Is this item ready for review?
        /// </summary>
        public bool IsDueForReview => DateTime.UtcNow >= NextReviewDate;

        /// <summary>
        /// Get days until next review
        /// </summary>
        public int DaysUntilNextReview
        {
            get
            {
                var days = (int)(NextReviewDate.Date - DateTime.UtcNow.Date).TotalDays;
                return Math.Max(0, days);
            }
        }

        /// <summary>
        /// Get review interval in days based on Leitner box
        /// </summary>
        public int GetReviewInterval()
        {
            return LeitnerBox switch
            {
                1 => 1,    // Box 1: 1 day
                2 => 3,    // Box 2: 3 days
                3 => 7,    // Box 3: 7 days
                4 => 14,   // Box 4: 14 days
                5 => 30,   // Box 5: 30 days (mastered)
                _ => 1
            };
        }

        /// <summary>
        /// Calculate next review date based on current box
        /// </summary>
        public void UpdateNextReviewDate()
        {
            LastReviewedDate = DateTime.UtcNow;
            NextReviewDate = DateTime.UtcNow.AddDays(GetReviewInterval());
        }

        /// <summary>
        /// Move item to next box if answered correctly
        /// </summary>
        public void CorrectAnswer()
        {
            CorrectCount++;
            if (LeitnerBox < 5)
            {
                LeitnerBox++;
            }
            UpdateNextReviewDate();
        }

        /// <summary>
        /// Reset item to box 1 if answered incorrectly
        /// </summary>
        public void IncorrectAnswer()
        {
            IncorrectCount++;
            LeitnerBox = 1; // Reset to box 1
            UpdateNextReviewDate();
        }

        // Navigation properties
        public VocabularyItem? VocabularyItem { get; set; }
        public LanguageItem? LanguageItem { get; set; }

        public override string ToString()
        {
            return $"Box {LeitnerBox}: Due {NextReviewDate:yyyy-MM-dd}";
        }
    }
}
