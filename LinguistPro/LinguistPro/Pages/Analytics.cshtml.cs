using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinguistPro.Models;
using LinguistPro.Services;
using System.Globalization;

namespace LinguistPro.Pages
{
    [Authorize]
    public class AnalyticsModel(
        UserManager<ApplicationUser> userManager,
        AppDbContext db,
        AnalyticsService analyticsService) : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AppDbContext _db = db;
        private readonly AnalyticsService _analyticsService = analyticsService;

        // Quick Stats
        public int TotalItems { get; set; }
        public int TotalLearningDays { get; set; }
        public double AverageMastery { get; set; }
        public int EstimatedDaysToFluency { get; set; }

        // Language Stats
        public List<LanguageAnalyticsViewModel> LanguageStats { get; set; } = [];

        // Chart Data
        public Dictionary<string, double> MasteryByItemType { get; set; } = [];
        public List<DailyLearningDataViewModel> DailyLearningData { get; set; } = [];
        public List<WeeklyStatsViewModel> WeeklyStats { get; set; } = [];

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            // Get user's language profiles
            var langProfiles = await _db.LanguageProfiles
                .Include(lp => lp.VocabularyItems)
                .Include(lp => lp.VerbEntries)
                .Include(lp => lp.LanguageItems)
                .Where(lp => lp.UserId == user.Id && lp.IsActive)
                .ToListAsync();

            // Calculate overall stats
            foreach (var langProfile in langProfiles)
            {
                var vocabCount = langProfile.VocabularyItems?.Count ?? 0;
                var verbCount = langProfile.VerbEntries?.Count ?? 0;
                var numberCount = langProfile.LanguageItems?.Count(i => i.ItemType == "Number") ?? 0;
                var monthCount = langProfile.LanguageItems?.Count(i => i.ItemType == "Month") ?? 0;
                var dayCount = langProfile.LanguageItems?.Count(i => i.ItemType == "Day") ?? 0;

                TotalItems += vocabCount + verbCount + numberCount + monthCount + dayCount;

                // Get daily learning logs
                var dailyLogs = await _db.DailyLearningLogs
                    .Where(d => d.LanguageProfileId == langProfile.LanguageProfileId)
                    .ToListAsync();

                TotalLearningDays += dailyLogs.Select(d => d.LearningDate.Date).Distinct().Count();

                // Get estimated days to fluency
                var estimatedDays = await _analyticsService.EstimateDaysToFluencyAsync(langProfile.LanguageProfileId, 90.0);
                if (estimatedDays > 0 && estimatedDays < 365)
                {
                    EstimatedDaysToFluency = estimatedDays;
                }

                // Build language analytics
                LanguageStats.Add(new LanguageAnalyticsViewModel
                {
                    LanguageName = langProfile.LanguageName ?? "Unknown",
                    LanguageCode = langProfile.LanguageCode ?? "unknown",
                    VocabCount = vocabCount,
                    VerbCount = verbCount,
                    NumberCount = numberCount,
                    MonthCount = monthCount,
                    DayCount = dayCount,
                    MasteryLevel = (int)langProfile.MasteryLevel,
                    EstimatedDaysToFluency = estimatedDays
                });
            }

            // Calculate average mastery
            if (LanguageStats.Count > 0)
            {
                AverageMastery = LanguageStats.Average(l => l.MasteryLevel);
            }

            // Get mastery by item type
            await GetMasteryByItemType(user.Id);

            // Get daily learning data (last 30 days)
            await GetDailyLearningData(user.Id);

            // Get weekly statistics (last 12 weeks)
            await GetWeeklyStats(user.Id);

            // Sort by item count
            LanguageStats = [.. LanguageStats.OrderByDescending(l => l.VocabCount + l.VerbCount + l.NumberCount + l.MonthCount + l.DayCount)];
        }

        private async Task GetMasteryByItemType(int userId)
        {
            var langProfiles = await _db.LanguageProfiles
                .Include(lp => lp.VocabularyItems)
                .Include(lp => lp.VerbEntries)
                .Include(lp => lp.LanguageItems)
                .Where(lp => lp.UserId == userId && lp.IsActive)
                .ToListAsync();

            double vocabMastery = 0;
            double verbMastery = 0;
            double numberMastery = 0;
            double monthMastery = 0;
            double dayMastery = 0;

            foreach (var langProfile in langProfiles)
            {
                if (langProfile.VocabularyItems?.Any() == true)
                    vocabMastery += langProfile.VocabularyItems.Average(v => v.Mastery);

                // Note: VerbEntries don't have mastery tracking in current model
                // Skip verb mastery calculation

                var numbers = langProfile.LanguageItems?.Where(i => i.ItemType == "Number").ToList() ?? [];
                if (numbers.Count != 0)
                    numberMastery += numbers.Average(i => i.Mastery);

                var months = langProfile.LanguageItems?.Where(i => i.ItemType == "Month").ToList() ?? [];
                if (months.Count != 0)
                    monthMastery += months.Average(i => i.Mastery);

                var days = langProfile.LanguageItems?.Where(i => i.ItemType == "Day").ToList() ?? [];
                if (days.Count != 0)
                    dayMastery += days.Average(i => i.Mastery);
            }

            if (langProfiles.Count > 0)
            {
                MasteryByItemType["Vocabulary"] = vocabMastery / langProfiles.Count;
                MasteryByItemType["Verbs"] = verbMastery / langProfiles.Count;
                MasteryByItemType["Numbers"] = numberMastery / langProfiles.Count;
                MasteryByItemType["Months"] = monthMastery / langProfiles.Count;
                MasteryByItemType["Days"] = dayMastery / langProfiles.Count;

                // Remove empty entries
                var emptyKeys = MasteryByItemType.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key).ToList();
                foreach (var key in emptyKeys)
                {
                    MasteryByItemType.Remove(key);
                }
            }
        }

        private async Task GetDailyLearningData(int userId)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var dailyLogs = await _db.DailyLearningLogs
                .Include(d => d.LanguageProfile)
                .Where(d => d.LanguageProfile != null && d.LanguageProfile.UserId == userId && d.LearningDate >= thirtyDaysAgo)
                .GroupBy(d => d.LearningDate.Date)
                .Select(g => new DailyLearningDataViewModel
                {
                    Date = g.Key,
                    ItemsLearned = g.Sum(d => d.GetTotalItemsLearned()),
                    TimeSpentSeconds = g.Sum(d => d.TotalTimeSpentSeconds)
                })
                .OrderBy(d => d.Date)
                .ToListAsync();

            DailyLearningData = dailyLogs;
        }

        private async Task GetWeeklyStats(int userId)
        {
            var twelveWeeksAgo = DateTime.UtcNow.AddDays(-84); // 12 weeks

            var dailyLogs = await _db.DailyLearningLogs
                .Include(d => d.LanguageProfile)
                .Where(d => d.LanguageProfile != null && d.LanguageProfile.UserId == userId && d.LearningDate >= twelveWeeksAgo)
                .ToListAsync();

            if (dailyLogs.Count == 0)
                return;

            // Group by week
            var weeklyGroups = dailyLogs
                .GroupBy(d => new { d.LearningDate.Year, Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.LearningDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                .OrderByDescending(g => g.Key)
                .Take(4)
                .ToList();

            foreach (var week in weeklyGroups)
            {
                var weekLogs = week.ToList();
                var avgMastery = weekLogs.Count > 0 
                    ? weekLogs.Average(d => d.LanguageProfile?.MasteryLevel ?? 0) 
                    : 0;

                WeeklyStats.Add(new WeeklyStatsViewModel
                {
                    Period = $"Week of {week.First().LearningDate.Date:MMM dd}",
                    ItemsReviewed = weekLogs.Sum(d => d.GetTotalItemsLearned()),
                    AverageMastery = avgMastery,
                    DaysActive = week.Select(d => d.LearningDate.Date).Distinct().Count(),
                    TotalTimeSpentSeconds = weekLogs.Sum(d => d.TotalTimeSpentSeconds)
                });
            }
        }
    }

    public class LanguageAnalyticsViewModel
    {
        public string LanguageName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public int VocabCount { get; set; } = 0;
        public int VerbCount { get; set; } = 0;
        public int NumberCount { get; set; } = 0;
        public int MonthCount { get; set; } = 0;
        public int DayCount { get; set; } = 0;
        public int MasteryLevel { get; set; } = 0;
        public int EstimatedDaysToFluency { get; set; } = 0;
    }

    public class DailyLearningDataViewModel
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int ItemsLearned { get; set; } = 0;
        public long TimeSpentSeconds { get; set; } = 0;
    }

    public class WeeklyStatsViewModel
    {
        public string Period { get; set; } = string.Empty;
        public int ItemsReviewed { get; set; } = 0;
        public double AverageMastery { get; set; } = 0;
        public int DaysActive { get; set; } = 0;
        public long TotalTimeSpentSeconds { get; set; } = 0;
    }
}
