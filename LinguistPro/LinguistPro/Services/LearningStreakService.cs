using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for managing learning streaks and consistency tracking.
    /// Handles streak creation, updates, resets, and calculations.
    /// </summary>
    public class LearningStreakService
    {
        private readonly AppDbContext _context;

        public LearningStreakService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get or create a learning streak for a language
        /// </summary>
        public async Task<LearningStreak> GetOrCreateStreakAsync(int languageProfileId)
        {
            var streak = await _context.LearningStreaks
                .FirstOrDefaultAsync(s => s.LanguageProfileId == languageProfileId);

            if (streak == null)
            {
                streak = new LearningStreak
                {
                    LanguageProfileId = languageProfileId,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    StreakStartDate = DateTime.UtcNow,
                    LastLearningDate = DateTime.MinValue,
                    TotalLearningDays = 0,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow
                };

                _context.LearningStreaks.Add(streak);
                await _context.SaveChangesAsync();
            }

            return streak;
        }

        /// <summary>
        /// Update streak when user learns something today
        /// </summary>
        public async Task<LearningStreak> UpdateStreakAsync(int languageProfileId)
        {
            var streak = await GetOrCreateStreakAsync(languageProfileId);
            var today = DateTime.UtcNow.Date;

            // Check if user already has learning activity today
            var todayLog = await _context.DailyLearningLogs
                .FirstOrDefaultAsync(d => d.LanguageProfileId == languageProfileId && 
                                         d.LearningDate.Date == today);

            // If already logged today, no need to update streak
            if (todayLog != null && todayLog.LearningDate.Date == today)
            {
                return streak;
            }

            var lastLearningDate = streak.LastLearningDate.Date;
            var daysSinceLastLearning = (today - lastLearningDate).TotalDays;

            // Update streak logic
            if (daysSinceLastLearning == 1)
            {
                // User learned yesterday, continue streak
                streak.CurrentStreak++;
            }
            else if (daysSinceLastLearning > 1)
            {
                // Streak was broken, reset to 1
                streak.CurrentStreak = 1;
                streak.StreakStartDate = DateTime.UtcNow;
            }
            else if (daysSinceLastLearning == 0)
            {
                // Already learned today, don't change streak
                return streak;
            }
            else
            {
                // First learning, start streak
                streak.CurrentStreak = 1;
                streak.StreakStartDate = DateTime.UtcNow;
            }

            // Update longest streak if current exceeds it
            if (streak.CurrentStreak > streak.LongestStreak)
            {
                streak.LongestStreak = streak.CurrentStreak;
            }

            streak.LastLearningDate = DateTime.UtcNow;
            streak.TotalLearningDays++;
            streak.LastUpdatedDate = DateTime.UtcNow;

            _context.LearningStreaks.Update(streak);
            await _context.SaveChangesAsync();

            return streak;
        }

        /// <summary>
        /// Log learning activity for a specific day
        /// </summary>
        public async Task<DailyLearningLog> LogLearningActivityAsync(
            int languageProfileId,
            int vocabularyCount = 0,
            int verbCount = 0,
            int languageItemCount = 0,
            int timeSpentSeconds = 0,
            int itemsMastered = 0,
            int masteryLevel = 0)
        {
            var today = DateTime.UtcNow.Date;

            // Check if log already exists for today
            var existingLog = await _context.DailyLearningLogs
                .FirstOrDefaultAsync(d => d.LanguageProfileId == languageProfileId && 
                                         d.LearningDate.Date == today);

            if (existingLog != null)
            {
                // Update existing log
                existingLog.VocabularyItemsLearned += vocabularyCount;
                existingLog.VerbsLearned += verbCount;
                existingLog.LanguageItemsLearned += languageItemCount;
                existingLog.TotalTimeSpentSeconds += timeSpentSeconds;
                existingLog.ItemsMastered += itemsMastered;
                existingLog.MasteryLevelAtEndOfDay = masteryLevel;
                existingLog.LastUpdatedDate = DateTime.UtcNow;

                _context.DailyLearningLogs.Update(existingLog);
            }
            else
            {
                // Create new log
                var newLog = new DailyLearningLog
                {
                    LanguageProfileId = languageProfileId,
                    LearningDate = DateTime.UtcNow,
                    VocabularyItemsLearned = vocabularyCount,
                    VerbsLearned = verbCount,
                    LanguageItemsLearned = languageItemCount,
                    TotalTimeSpentSeconds = timeSpentSeconds,
                    ItemsMastered = itemsMastered,
                    MasteryLevelAtEndOfDay = masteryLevel,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow
                };

                _context.DailyLearningLogs.Add(newLog);
                existingLog = newLog;
            }

            await _context.SaveChangesAsync();

            // Update streak after logging
            await UpdateStreakAsync(languageProfileId);

            return existingLog;
        }

        /// <summary>
        /// Get learning statistics for a language
        /// </summary>
        public async Task<StreakStatistics> GetStreakStatisticsAsync(int languageProfileId)
        {
            var streak = await GetOrCreateStreakAsync(languageProfileId);

            var logs = await _context.DailyLearningLogs
                .Where(d => d.LanguageProfileId == languageProfileId)
                .OrderByDescending(d => d.LearningDate)
                .ToListAsync();

            var stats = new StreakStatistics
            {
                CurrentStreak = streak.CurrentStreak,
                LongestStreak = streak.LongestStreak,
                TotalLearningDays = streak.TotalLearningDays,
                StreakStartDate = streak.StreakStartDate,
                LastLearningDate = streak.LastLearningDate,
                IsStreakActive = streak.IsStreakActive(),
                StreakBadge = streak.GetStreakBadge(),
                TotalItemsLearned = logs.Sum(l => l.GetTotalItemsLearned()),
                TotalTimeSpentSeconds = logs.Sum(l => l.TotalTimeSpentSeconds),
                AverageDailyItemsLearned = logs.Any() ? logs.Average(l => l.GetTotalItemsLearned()) : 0,
                DaysUntilNextMilestone = GetDaysUntilNextMilestone(streak.CurrentStreak)
            };

            return stats;
        }

        /// <summary>
        /// Get milestone information
        /// </summary>
        public int GetDaysUntilNextMilestone(int currentStreak)
        {
            return currentStreak switch
            {
                < 7 => 7 - currentStreak,
                < 30 => 30 - currentStreak,
                < 100 => 100 - currentStreak,
                _ => int.MaxValue
            };
        }

        /// <summary>
        /// Get current milestone
        /// </summary>
        public string GetCurrentMilestone(int currentStreak)
        {
            return currentStreak switch
            {
                >= 100 => "🏆 Legendary (100+ days)",
                >= 30 => "🌟 Master (30+ days)",
                >= 7 => "🔥 Dedicated (7+ days)",
                >= 1 => "🚀 Started (1+ days)",
                _ => "📚 Beginning"
            };
        }

        /// <summary>
        /// Check if user earned a milestone today
        /// </summary>
        public bool IsMilestoneAchieved(int currentStreak)
        {
            return currentStreak == 7 || currentStreak == 30 || currentStreak == 100;
        }

        /// <summary>
        /// Reset streak (useful for admin or user request)
        /// </summary>
        public async Task ResetStreakAsync(int languageProfileId)
        {
            var streak = await GetOrCreateStreakAsync(languageProfileId);
            
            streak.CurrentStreak = 0;
            streak.StreakStartDate = DateTime.UtcNow;
            streak.LastUpdatedDate = DateTime.UtcNow;

            _context.LearningStreaks.Update(streak);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Data class for streak statistics
    /// </summary>
    public class StreakStatistics
    {
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int TotalLearningDays { get; set; }
        public DateTime StreakStartDate { get; set; }
        public DateTime LastLearningDate { get; set; }
        public bool IsStreakActive { get; set; }
        public string StreakBadge { get; set; } = "";
        public int TotalItemsLearned { get; set; }
        public int TotalTimeSpentSeconds { get; set; }
        public double AverageDailyItemsLearned { get; set; }
        public int DaysUntilNextMilestone { get; set; }
    }
}
