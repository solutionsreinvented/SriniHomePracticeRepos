using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinguistPro.Models;

namespace LinguistPro.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public DashboardModel(UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public string UserFullName { get; set; } = string.Empty;
        public int VocabCount { get; set; }
        public int VerbCount { get; set; }
        public int NumberCount { get; set; }
        public int MonthCount { get; set; }
        public int DayCount { get; set; }
        public int TotalItems { get; set; }
        public int TotalLanguages { get; set; }
        public double AverageMastery { get; set; }
        public List<LanguageStatsViewModel> LanguageStats { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            UserFullName = $"{user.FirstName} {user.LastName}".Trim();

            // Get user's language profiles
            var langProfiles = await _db.LanguageProfiles
                .Where(lp => lp.UserId == user.Id)
                .ToListAsync();

            TotalLanguages = langProfiles.Count;

            // Calculate stats for each language
            foreach (var langProfile in langProfiles)
            {
                var vocabCount = await _db.Vocabulary
                    .Where(v => v.LanguageProfileId == langProfile.LanguageProfileId)
                    .CountAsync();

                var verbCount = await _db.Verbs
                    .Where(v => v.LanguageProfileId == langProfile.LanguageProfileId)
                    .CountAsync();

                var numberCount = await _db.LanguageItems
                    .Where(i => i.LanguageProfileId == langProfile.LanguageProfileId && i.ItemType == "Number")
                    .CountAsync();

                var monthCount = await _db.LanguageItems
                    .Where(i => i.LanguageProfileId == langProfile.LanguageProfileId && i.ItemType == "Month")
                    .CountAsync();

                var dayCount = await _db.LanguageItems
                    .Where(i => i.LanguageProfileId == langProfile.LanguageProfileId && i.ItemType == "Day")
                    .CountAsync();

                int totalForLang = vocabCount + verbCount + numberCount + monthCount + dayCount;

                LanguageStats.Add(new LanguageStatsViewModel
                {
                    LanguageName = langProfile.LanguageName,
                    LanguageCode = langProfile.LanguageCode,
                    VocabCount = vocabCount,
                    VerbCount = verbCount,
                    NumberCount = numberCount,
                    MonthCount = monthCount,
                    DayCount = dayCount,
                    ItemCount = totalForLang,
                    MasteryLevel = langProfile.MasteryLevel,
                    IsActive = langProfile.IsActive
                });

                VocabCount += vocabCount;
                VerbCount += verbCount;
                NumberCount += numberCount;
                MonthCount += monthCount;
                DayCount += dayCount;
                TotalItems += totalForLang;
            }

            // Calculate average mastery
            if (LanguageStats.Count > 0)
            {
                AverageMastery = LanguageStats.Average(l => l.MasteryLevel);
            }

            // Sort by item count descending
            LanguageStats = LanguageStats.OrderByDescending(l => l.ItemCount).ToList();
        }
    }

    public class LanguageStatsViewModel
    {
        public string LanguageName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public int VocabCount { get; set; }
        public int VerbCount { get; set; }
        public int NumberCount { get; set; }
        public int MonthCount { get; set; }
        public int DayCount { get; set; }
        public int ItemCount { get; set; }
        public int MasteryLevel { get; set; }
        public bool IsActive { get; set; }
    }
}
