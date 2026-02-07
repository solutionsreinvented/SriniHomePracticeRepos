using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for analytics, progress tracking, and dashboard data generation.
    /// Provides comprehensive learning metrics and trend analysis.
    /// </summary>
    public class AnalyticsService(AppDbContext context, LearningStreakService streakService)
    {
        private readonly AppDbContext _context = context;
        private readonly LearningStreakService _streakService = streakService;

        /// <summary>
        /// Take a snapshot of current progress for a language
        /// </summary>
        public async Task<ProgressSnapshot> TakeProgressSnapshotAsync(int languageProfileId, string periodType = "Daily")
        {
            var langProfile = await _context.LanguageProfiles
                .Include(l => l.VocabularyItems)
                .Include(l => l.LanguageItems)
                .Include(l => l.VerbEntries)
                .FirstOrDefaultAsync(l => l.LanguageProfileId == languageProfileId) ?? throw new InvalidOperationException("Language profile not found");
            var totalItems = (langProfile.VocabularyItems?.Count ?? 0) + 
                            (langProfile.VerbEntries?.Count ?? 0) + 
                            (langProfile.LanguageItems?.Count ?? 0);

            var masteredItems = (langProfile.VocabularyItems?.Count(v => v.Mastery >= 90) ?? 0) +
                               (langProfile.LanguageItems?.Count(i => i.Mastery >= 90) ?? 0);

            var averageMastery = totalItems > 0
                ? ((langProfile.VocabularyItems?.Sum(v => v.Mastery) ?? 0) +
                   (langProfile.LanguageItems?.Sum(i => i.Mastery) ?? 0)) / (double)totalItems
                : 0;

            var dailyLogs = await _context.DailyLearningLogs
                .Where(d => d.LanguageProfileId == languageProfileId)
                .OrderByDescending(d => d.LearningDate)
                .Take(30)
                .ToListAsync();

            var totalTimeSeconds = dailyLogs.Sum(d => d.TotalTimeSpentSeconds);
            var itemsReviewed = dailyLogs.Sum(d => d.GetTotalItemsLearned());
            var retentionRate = totalItems > 0 ? (masteredItems / (double)totalItems) * 100 : 0;

            var streak = await _streakService.GetOrCreateStreakAsync(languageProfileId);

            var snapshot = new ProgressSnapshot
            {
                LanguageProfileId = languageProfileId,
                SnapshotDate = DateTime.UtcNow,
                PeriodType = periodType,
                TotalItems = totalItems,
                ItemsAdded = 0, // Will be calculated based on period
                AverageMastery = averageMastery,
                ItemsMastered = masteredItems,
                TotalTimeSpentSeconds = totalTimeSeconds,
                CurrentStreak = streak.CurrentStreak,
                ItemsReviewed = itemsReviewed,
                RetentionRate = retentionRate,
                CreatedDate = DateTime.UtcNow
            };

            _context.ProgressSnapshots.Add(snapshot);
            await _context.SaveChangesAsync();

            return snapshot;
        }

        /// <summary>
        /// Get progress trend over last N days
        /// </summary>
        public async Task<List<ProgressSnapshot>> GetProgressTrendAsync(int languageProfileId, int daysBack = 30)
        {
            var startDate = DateTime.UtcNow.AddDays(-daysBack);

            var snapshots = await _context.ProgressSnapshots
                .Where(s => s.LanguageProfileId == languageProfileId && 
                           s.SnapshotDate >= startDate &&
                           s.PeriodType == "Daily")
                .OrderBy(s => s.SnapshotDate)
                .ToListAsync();

            return snapshots;
        }

        /// <summary>
        /// Get weekly progress trend
        /// </summary>
        public async Task<List<ProgressSnapshot>> GetWeeklyProgressAsync(int languageProfileId, int weeksBack = 12)
        {
            var startDate = DateTime.UtcNow.AddDays(-weeksBack * 7);

            var snapshots = await _context.ProgressSnapshots
                .Where(s => s.LanguageProfileId == languageProfileId && 
                           s.SnapshotDate >= startDate &&
                           s.PeriodType == "Weekly")
                .OrderBy(s => s.SnapshotDate)
                .ToListAsync();

            return snapshots;
        }

        /// <summary>
        /// Calculate estimated days to fluency
        /// Based on current mastery rate and learning velocity
        /// </summary>
        public async Task<int> EstimateDaysToFluencyAsync(int languageProfileId, double targetMastery = 90.0)
        {
            var langProfile = await _context.LanguageProfiles
                .Include(l => l.VocabularyItems)
                .Include(l => l.LanguageItems)
                .Include(l => l.VerbEntries)
                .FirstOrDefaultAsync(l => l.LanguageProfileId == languageProfileId);

            if (langProfile == null)
                return 0;

            var totalItems = (langProfile.VocabularyItems?.Count ?? 0) + 
                            (langProfile.VerbEntries?.Count ?? 0) + 
                            (langProfile.LanguageItems?.Count ?? 0);

            if (totalItems == 0)
                return 0;

            var currentMastery = langProfile.MasteryLevel;
            var masteryNeeded = targetMastery - currentMastery;

            if (masteryNeeded <= 0)
                return 0;

            // Get learning velocity from last 30 days
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentLogs = await _context.DailyLearningLogs
                .Where(d => d.LanguageProfileId == languageProfileId && d.LearningDate >= thirtyDaysAgo)
                .ToListAsync();

            if (recentLogs.Count == 0)
                return int.MaxValue; // No data, can't estimate

            var avgMasteryGainPerDay = recentLogs.Count != 0
                ? masteryNeeded / recentLogs.Count 
                : 0.5; // Default: 0.5% per day

            var daysEstimated = (int)Math.Ceiling(masteryNeeded / Math.Max(avgMasteryGainPerDay, 0.1));

            return Math.Min(daysEstimated, 365); // Cap at 1 year
        }

        /// <summary>
        /// Get learning statistics
        /// </summary>
        public async Task<LearningStatistics> GetLearningStatisticsAsync(int languageProfileId)
        {
            var langProfile = await _context.LanguageProfiles
                .Include(l => l.VocabularyItems)
                .Include(l => l.LanguageItems)
                .Include(l => l.VerbEntries)
                .FirstOrDefaultAsync(l => l.LanguageProfileId == languageProfileId) ?? throw new InvalidOperationException("Language profile not found");
            var dailyLogs = await _context.DailyLearningLogs
                .Where(d => d.LanguageProfileId == languageProfileId)
                .ToListAsync();

            var totalItems = (langProfile.VocabularyItems?.Count ?? 0) + 
                            (langProfile.VerbEntries?.Count ?? 0) + 
                            (langProfile.LanguageItems?.Count ?? 0);

            var vocabMastery = langProfile.VocabularyItems?.Average(v => v.Mastery) ?? 0;
            var verbMastery = 0; // VerbEntries don't have mastery tracking
            var itemMastery = langProfile.LanguageItems?.Average(i => i.Mastery) ?? 0;

            var stats = new LearningStatistics
            {
                TotalVocabularyItems = langProfile.VocabularyItems?.Count ?? 0,
                TotalVerbs = langProfile.VerbEntries?.Count ?? 0,
                TotalLanguageItems = langProfile.LanguageItems?.Count ?? 0,
                OverallMastery = langProfile.MasteryLevel,
                VocabularyMastery = vocabMastery,
                VerbMastery = verbMastery,
                LanguageItemMastery = itemMastery,
                TotalLearningDays = dailyLogs.Count,
                TotalTimeSpentSeconds = dailyLogs.Sum(d => d.TotalTimeSpentSeconds),
                AverageDailyTimeSeconds = dailyLogs.Count != 0 
                    ? (long)dailyLogs.Average(d => d.TotalTimeSpentSeconds) 
                    : 0,
                ItemsReviewedThisWeek = dailyLogs
                    .Where(d => d.LearningDate >= DateTime.UtcNow.AddDays(-7))
                    .Sum(d => d.GetTotalItemsLearned()),
                LastLearningDate = dailyLogs.Count > 0 ? dailyLogs.Max(d => d.LearningDate) : DateTime.UtcNow
            };

            return stats;
        }

        /// <summary>
        /// Get mastery level by item type
        /// </summary>
        public async Task<Dictionary<string, double>> GetMasteryByTypeAsync(int languageProfileId)
        {
            var langProfile = await _context.LanguageProfiles
                .Include(l => l.VocabularyItems)
                .Include(l => l.LanguageItems)
                .Include(l => l.VerbEntries)
                .FirstOrDefaultAsync(l => l.LanguageProfileId == languageProfileId);

            if (langProfile == null)
                return [];

            var masteryByType = new Dictionary<string, double>
            {
                { "Vocabulary", langProfile.VocabularyItems?.Any() == true 
                    ? langProfile.VocabularyItems.Average(v => v.Mastery) 
                    : 0 },
                { "Verbs", 0 }, // VerbEntries don't have mastery tracking
                { "Numbers", langProfile.LanguageItems?.Where(i => i.ItemType == "Number").Any() == true 
                    ? langProfile.LanguageItems.Where(i => i.ItemType == "Number").Average(i => i.Mastery) 
                    : 0 },
                { "Months", langProfile.LanguageItems?.Where(i => i.ItemType == "Month").Any() == true 
                    ? langProfile.LanguageItems.Where(i => i.ItemType == "Month").Average(i => i.Mastery) 
                    : 0 },
                { "Days", langProfile.LanguageItems?.Where(i => i.ItemType == "Day").Any() == true 
                    ? langProfile.LanguageItems.Where(i => i.ItemType == "Day").Average(i => i.Mastery) 
                    : 0 }
            };

            return masteryByType;
        }

        /// <summary>
        /// Get items due for review by difficulty
        /// </summary>
        public async Task<ReviewDifficultyDistribution> GetReviewDifficultyAsync(int languageProfileId)
        {
            var dueCount = await _context.ReviewSchedules
                .Where(r => (r.VocabularyItem != null && r.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (r.LanguageItem != null && r.LanguageItem.LanguageProfileId == languageProfileId))
                .CountAsync(r => r.IsDueForReview);

            var easyCount = await _context.ReviewSchedules
                .Where(r => (r.VocabularyItem != null && r.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (r.LanguageItem != null && r.LanguageItem.LanguageProfileId == languageProfileId))
                .Where(r => r.IsDueForReview && r.LeitnerBox >= 3)
                .CountAsync();

            var hardCount = await _context.ReviewSchedules
                .Where(r => (r.VocabularyItem != null && r.VocabularyItem.LanguageProfileId == languageProfileId) ||
                           (r.LanguageItem != null && r.LanguageItem.LanguageProfileId == languageProfileId))
                .Where(r => r.IsDueForReview && r.LeitnerBox <= 2)
                .CountAsync();

            return new ReviewDifficultyDistribution
            {
                TotalDueForReview = dueCount,
                EasyItems = easyCount,
                HardItems = hardCount
            };
        }
    }

    /// <summary>
    /// Learning statistics summary
    /// </summary>
    public class LearningStatistics
    {
        public int TotalVocabularyItems { get; set; }
        public int TotalVerbs { get; set; }
        public int TotalLanguageItems { get; set; }
        public double OverallMastery { get; set; }
        public double VocabularyMastery { get; set; }
        public double VerbMastery { get; set; }
        public double LanguageItemMastery { get; set; }
        public int TotalLearningDays { get; set; }
        public long TotalTimeSpentSeconds { get; set; }
        public long AverageDailyTimeSeconds { get; set; }
        public int ItemsReviewedThisWeek { get; set; }
        public DateTime LastLearningDate { get; set; }

        public int TotalItems => TotalVocabularyItems + TotalVerbs + TotalLanguageItems;
        
        public string GetFormattedTotalTime()
        {
            var span = TimeSpan.FromSeconds(TotalTimeSpentSeconds);
            if (span.TotalHours >= 1)
                return $"{span.TotalHours:F1}h";
            return $"{span.TotalMinutes:F0}m";
        }
    }

    /// <summary>
    /// Review difficulty distribution
    /// </summary>
    public class ReviewDifficultyDistribution
    {
        public int TotalDueForReview { get; set; }
        public int EasyItems { get; set; }
        public int HardItems { get; set; }
    }
}
