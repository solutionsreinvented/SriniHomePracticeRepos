using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for managing spaced repetition learning using the Leitner Algorithm.
    /// Implements scientifically-proven SRS (Spaced Repetition System) for optimal learning retention.
    /// </summary>
    public class SpacedRepetitionService
    {
        private readonly AppDbContext _context;

        public SpacedRepetitionService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get or create a review schedule for a vocabulary item
        /// </summary>
        public async Task<ReviewSchedule> GetOrCreateVocabularyScheduleAsync(int vocabularyItemId)
        {
            var schedule = await _context.ReviewSchedules
                .FirstOrDefaultAsync(s => s.VocabularyItemId == vocabularyItemId);

            if (schedule == null)
            {
                schedule = new ReviewSchedule
                {
                    VocabularyItemId = vocabularyItemId,
                    LeitnerBox = 1,
                    CorrectCount = 0,
                    IncorrectCount = 0,
                    LastReviewedDate = DateTime.UtcNow,
                    NextReviewDate = DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow
                };

                _context.ReviewSchedules.Add(schedule);
                await _context.SaveChangesAsync();
            }

            return schedule;
        }

        /// <summary>
        /// Get or create a review schedule for a language item
        /// </summary>
        public async Task<ReviewSchedule> GetOrCreateLanguageItemScheduleAsync(int languageItemId)
        {
            var schedule = await _context.ReviewSchedules
                .FirstOrDefaultAsync(s => s.LanguageItemId == languageItemId);

            if (schedule == null)
            {
                schedule = new ReviewSchedule
                {
                    LanguageItemId = languageItemId,
                    LeitnerBox = 1,
                    CorrectCount = 0,
                    IncorrectCount = 0,
                    LastReviewedDate = DateTime.UtcNow,
                    NextReviewDate = DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow
                };

                _context.ReviewSchedules.Add(schedule);
                await _context.SaveChangesAsync();
            }

            return schedule;
        }

        /// <summary>
        /// Get all items due for review in a language profile
        /// </summary>
        public async Task<List<ReviewSchedule>> GetItemsDueForReviewAsync(int languageProfileId)
        {
            var today = DateTime.UtcNow.Date;

            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .Where(s => s.NextReviewDate.Date <= today)
                .OrderBy(s => s.NextReviewDate)
                .Include(s => s.VocabularyItem)
                .Include(s => s.LanguageItem)
                .ToListAsync();

            return schedules;
        }

        /// <summary>
        /// Get upcoming items scheduled for review (next 7 days)
        /// </summary>
        public async Task<List<ReviewSchedule>> GetUpcomingReviewsAsync(int languageProfileId, int daysAhead = 7)
        {
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(daysAhead);

            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .Where(s => s.NextReviewDate.Date > today && s.NextReviewDate.Date <= endDate)
                .OrderBy(s => s.NextReviewDate)
                .Include(s => s.VocabularyItem)
                .Include(s => s.LanguageItem)
                .ToListAsync();

            return schedules;
        }

        /// <summary>
        /// Record correct answer and move item to next Leitner box
        /// </summary>
        public async Task<ReviewSchedule> RecordCorrectReviewAsync(int scheduleId)
        {
            var schedule = await _context.ReviewSchedules.FindAsync(scheduleId);
            if (schedule == null)
                throw new InvalidOperationException("Schedule not found");

            schedule.CorrectAnswer();
            _context.ReviewSchedules.Update(schedule);
            await _context.SaveChangesAsync();

            return schedule;
        }

        /// <summary>
        /// Record incorrect answer and reset item to box 1
        /// </summary>
        public async Task<ReviewSchedule> RecordIncorrectReviewAsync(int scheduleId)
        {
            var schedule = await _context.ReviewSchedules.FindAsync(scheduleId);
            if (schedule == null)
                throw new InvalidOperationException("Schedule not found");

            schedule.IncorrectAnswer();
            _context.ReviewSchedules.Update(schedule);
            await _context.SaveChangesAsync();

            return schedule;
        }

        /// <summary>
        /// Get review statistics for a language
        /// </summary>
        public async Task<SRSStatistics> GetSRSStatisticsAsync(int languageProfileId)
        {
            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .ToListAsync();

            var dueReviews = schedules.Count(s => s.IsDueForReview);
            var masteredItems = schedules.Count(s => s.LeitnerBox == 5);
            var totalItems = schedules.Count;
            var averageCorrectness = totalItems > 0 
                ? (double)schedules.Sum(s => s.CorrectCount) / (schedules.Sum(s => s.CorrectCount + s.IncorrectCount) + 1)
                : 0;

            var stats = new SRSStatistics
            {
                TotalItems = totalItems,
                DueForReview = dueReviews,
                MasteredItems = masteredItems,
                AverageCorrectness = averageCorrectness,
                Box1Count = schedules.Count(s => s.LeitnerBox == 1),
                Box2Count = schedules.Count(s => s.LeitnerBox == 2),
                Box3Count = schedules.Count(s => s.LeitnerBox == 3),
                Box4Count = schedules.Count(s => s.LeitnerBox == 4),
                Box5Count = masteredItems,
                EstimatedReviewTime = CalculateEstimatedReviewTime(schedules)
            };

            return stats;
        }

        /// <summary>
        /// Estimate time needed to review all due items (in minutes)
        /// Assuming ~10 seconds per item
        /// </summary>
        private int CalculateEstimatedReviewTime(List<ReviewSchedule> schedules)
        {
            var dueCount = schedules.Count(s => s.IsDueForReview);
            return Math.Max(1, (dueCount * 10) / 60); // Convert to minutes
        }

        /// <summary>
        /// Get retention rate (correct / total reviews)
        /// </summary>
        public async Task<double> GetRetentionRateAsync(int languageProfileId)
        {
            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .ToListAsync();

            if (!schedules.Any())
                return 0;

            var totalReviews = schedules.Sum(s => s.CorrectCount + s.IncorrectCount);
            if (totalReviews == 0)
                return 0;

            var correctReviews = schedules.Sum(s => s.CorrectCount);
            return (double)correctReviews / totalReviews;
        }

        /// <summary>
        /// Get mastery percentage
        /// </summary>
        public async Task<double> GetMasteryPercentageAsync(int languageProfileId)
        {
            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .ToListAsync();

            if (!schedules.Any())
                return 0;

            var masteredCount = schedules.Count(s => s.LeitnerBox >= 5);
            return ((double)masteredCount / schedules.Count) * 100;
        }

        /// <summary>
        /// Get detailed review schedule information for UI display
        /// </summary>
        public async Task<ReviewScheduleInfo?> GetReviewScheduleInfoAsync(int scheduleId)
        {
            var schedule = await _context.ReviewSchedules
                .Include(s => s.VocabularyItem)
                .Include(s => s.LanguageItem)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null)
                return null;

            var term = schedule.VocabularyItem?.Term ?? schedule.LanguageItem?.Term ?? "Unknown";
            var meaning = schedule.VocabularyItem?.Meaning ?? schedule.LanguageItem?.Meaning ?? "Unknown";

            return new ReviewScheduleInfo
            {
                ScheduleId = schedule.ScheduleId,
                Term = term,
                Meaning = meaning,
                CurrentBox = schedule.LeitnerBox,
                CorrectCount = schedule.CorrectCount,
                IncorrectCount = schedule.IncorrectCount,
                NextReviewDate = schedule.NextReviewDate,
                DaysUntilReview = schedule.DaysUntilNextReview,
                IsDueForReview = schedule.IsDueForReview,
                RetentionPercentage = schedule.CorrectCount + schedule.IncorrectCount > 0
                    ? (double)schedule.CorrectCount / (schedule.CorrectCount + schedule.IncorrectCount) * 100
                    : 0
            };
        }

        /// <summary>
        /// Reset all schedules for a language (fresh start)
        /// </summary>
        public async Task ResetAllSchedulesAsync(int languageProfileId)
        {
            var schedules = await _context.ReviewSchedules
                .Where(s => (s.VocabularyItem != null && s.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (s.LanguageItem != null && s.LanguageItem.LanguageProfileId == languageProfileId))
                .ToListAsync();

            foreach (var schedule in schedules)
            {
                schedule.LeitnerBox = 1;
                schedule.CorrectCount = 0;
                schedule.IncorrectCount = 0;
                schedule.LastReviewedDate = DateTime.UtcNow;
                schedule.NextReviewDate = DateTime.UtcNow;
            }

            _context.ReviewSchedules.UpdateRange(schedules);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Spaced Repetition System Statistics
    /// </summary>
    public class SRSStatistics
    {
        public int TotalItems { get; set; }
        public int DueForReview { get; set; }
        public int MasteredItems { get; set; }
        public double AverageCorrectness { get; set; }
        public int Box1Count { get; set; }
        public int Box2Count { get; set; }
        public int Box3Count { get; set; }
        public int Box4Count { get; set; }
        public int Box5Count { get; set; }
        public int EstimatedReviewTime { get; set; }

        public double MasteryPercentage => TotalItems > 0 ? (double)MasteredItems / TotalItems * 100 : 0;
        public double ReviewCoveragePercentage => TotalItems > 0 ? (double)DueForReview / TotalItems * 100 : 0;
    }

    /// <summary>
    /// Review schedule information for display
    /// </summary>
    public class ReviewScheduleInfo
    {
        public int ScheduleId { get; set; }
        public required string Term { get; set; }
        public required string Meaning { get; set; }
        public int CurrentBox { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public DateTime NextReviewDate { get; set; }
        public int DaysUntilReview { get; set; }
        public bool IsDueForReview { get; set; }
        public double RetentionPercentage { get; set; }
    }
}
