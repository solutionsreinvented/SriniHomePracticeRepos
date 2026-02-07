# LinguistPro - Quick Implementation Guides

## 🎯 Option C: Advanced Features Implementation Guide

This guide provides step-by-step instructions for implementing the advanced features mentioned in Option C.

---

## 1️⃣ Learning Streaks Tracking

### **Database Changes Required:**

```csharp
// Add to LanguageProfile.cs
public class LanguageProfile
{
    // ... existing properties ...
    
    public int CurrentStreak { get; set; } = 0;
    public int LongestStreak { get; set; } = 0;
    public DateTime LastActivityDate { get; set; } = DateTime.MinValue;
    public DateTime StreakStartDate { get; set; } = DateTime.UtcNow;
}
```

### **Service Class to Add:**

Create `Services/StreakService.cs`:
```csharp
using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    public class StreakService
    {
        private readonly AppDbContext _db;

        public StreakService(AppDbContext db) => _db = db;

        public async Task UpdateStreakAsync(int languageProfileId)
        {
            var profile = await _db.LanguageProfiles.FindAsync(languageProfileId);
            if (profile == null) return;

            var today = DateTime.UtcNow.Date;
            var lastActivity = profile.LastActivityDate.Date;

            if (lastActivity == today)
                return; // Already updated today

            if (lastActivity == today.AddDays(-1))
            {
                // Streak continues
                profile.CurrentStreak++;
            }
            else if (lastActivity < today.AddDays(-1))
            {
                // Streak broken, restart
                profile.CurrentStreak = 1;
                profile.StreakStartDate = DateTime.UtcNow;
            }

            if (profile.CurrentStreak > profile.LongestStreak)
                profile.LongestStreak = profile.CurrentStreak;

            profile.LastActivityDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task<(int current, int longest)> GetStreaksAsync(int languageProfileId)
        {
            var profile = await _db.LanguageProfiles.FindAsync(languageProfileId);
            return profile == null ? (0, 0) : (profile.CurrentStreak, profile.LongestStreak);
        }
    }
}
```

### **Frontend - Add to Dashboard.cshtml:**

```html
<!-- Streaks Section -->
<div class="stat-card">
    <div class="stat-header">
        <span class="stat-icon">🔥</span>
    </div>
    <div class="stat-number">@Model.CurrentStreak</div>
    <div class="stat-label">Current Streak</div>
    <div class="stat-footer">
        Best streak: @Model.LongestStreak days
    </div>
</div>
```

### **Migration Command:**
```bash
dotnet ef migrations add AddStreakTracking
dotnet ef database update
```

---

## 2️⃣ Spaced Repetition System (Leitner Algorithm)

### **Database Changes:**

```csharp
// Add to VocabularyItem.cs
public class VocabularyItem
{
    // ... existing properties ...
    
    public int Box { get; set; } = 1; // Leitner boxes: 1-5
    public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;
    public int ReviewCount { get; set; } = 0;
    public int CorrectCount { get; set; } = 0;
}
```

### **Spaced Repetition Service:**

Create `Services/SpacedRepetitionService.cs`:
```csharp
using LinguistPro.Models;

namespace LinguistPro.Services
{
    public class SpacedRepetitionService
    {
        private readonly AppDbContext _db;

        // Leitner algorithm: box -> days until next review
        private static readonly Dictionary<int, int> BoxDays = new()
        {
            { 1, 1 },   // Review tomorrow
            { 2, 3 },   // Review in 3 days
            { 3, 7 },   // Review in 1 week
            { 4, 14 },  // Review in 2 weeks
            { 5, 30 }   // Review in 1 month
        };

        public SpacedRepetitionService(AppDbContext db) => _db = db;

        public async Task<List<VocabularyItem>> GetItemsDueForReviewAsync(int languageProfileId)
        {
            return await _db.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId &&
                            v.NextReviewDate <= DateTime.UtcNow)
                .OrderBy(v => v.NextReviewDate)
                .ToListAsync();
        }

        public async Task MarkCorrectAsync(int vocabularyId)
        {
            var item = await _db.Vocabulary.FindAsync(vocabularyId);
            if (item == null) return;

            item.CorrectCount++;
            item.ReviewCount++;

            if (item.Box < 5)
                item.Box++;

            item.NextReviewDate = DateTime.UtcNow.AddDays(BoxDays[item.Box]);
            await _db.SaveChangesAsync();
        }

        public async Task MarkIncorrectAsync(int vocabularyId)
        {
            var item = await _db.Vocabulary.FindAsync(vocabularyId);
            if (item == null) return;

            item.ReviewCount++;
            item.Box = 1; // Reset to box 1

            item.NextReviewDate = DateTime.UtcNow.AddDays(1);
            await _db.SaveChangesAsync();
        }

        public async Task<double> GetRetentionRateAsync(int languageProfileId)
        {
            var items = await _db.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId && v.ReviewCount > 0)
                .ToListAsync();

            if (!items.Any()) return 0;

            var correctCount = items.Sum(i => i.CorrectCount);
            var totalCount = items.Sum(i => i.ReviewCount);

            return (double)correctCount / totalCount * 100;
        }
    }
}
```

### **Migration:**
```bash
dotnet ef migrations add AddSpacedRepetitionFields
dotnet ef database update
```

---

## 3️⃣ Progress Charts & Analytics

### **Create Analytics Service:**

Create `Services/AnalyticsService.cs`:
```csharp
using LinguistPro.Models;

namespace LinguistPro.Services
{
    public class AnalyticsService
    {
        private readonly AppDbContext _db;

        public AnalyticsService(AppDbContext db) => _db = db;

        public async Task<Dictionary<string, int>> GetItemCountByLanguageAsync(int userId)
        {
            return await _db.LanguageProfiles
                .Where(lp => lp.UserId == userId)
                .Select(lp => new
                {
                    lp.LanguageName,
                    Count = lp.VocabularyItems.Count + lp.VerbEntries.Count + lp.LanguageItems.Count
                })
                .ToDictionaryAsync(x => x.LanguageName, x => x.Count);
        }

        public async Task<Dictionary<DateTime, int>> GetDailyLearningActivityAsync(int userId, int days = 30)
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;
            
            return await _db.VocabularyItems
                .Where(v => v.LanguageProfile!.UserId == userId && v.CreatedDate >= startDate)
                .GroupBy(v => v.CreatedDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count);
        }

        public async Task<List<LanguageMasteryDto>> GetMasteryTrendsAsync(int userId)
        {
            return await _db.LanguageProfiles
                .Where(lp => lp.UserId == userId && lp.IsActive)
                .Select(lp => new LanguageMasteryDto
                {
                    LanguageName = lp.LanguageName,
                    MasteryLevel = lp.MasteryLevel,
                    TotalItems = lp.VocabularyItems.Count + lp.VerbEntries.Count + lp.LanguageItems.Count
                })
                .ToListAsync();
        }
    }

    public class LanguageMasteryDto
    {
        public string LanguageName { get; set; } = string.Empty;
        public int MasteryLevel { get; set; }
        public int TotalItems { get; set; }
    }
}
```

### **Dashboard Page Model Addition:**

Add to `Pages/Dashboard.cshtml.cs`:
```csharp
public Dictionary<string, int> ItemCountByLanguage { get; set; } = new();
public Dictionary<DateTime, int> DailyActivity { get; set; } = new();
public List<LanguageMasteryDto> MasteryTrends { get; set; } = new();

// In OnGetAsync():
var analytics = new AnalyticsService(_db);
ItemCountByLanguage = await analytics.GetItemCountByLanguageAsync(user.Id);
DailyActivity = await analytics.GetDailyLearningActivityAsync(user.Id);
MasteryTrends = await analytics.GetMasteryTrendsAsync(user.Id);
```

### **HTML - Add Chart.js:**

Add to Dashboard layout:
```html
<!-- Add to _Layout.cshtml <head> -->
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<!-- In Dashboard.cshtml -->
<div style="width: 100%; max-width: 600px; margin: 40px auto;">
    <canvas id="masteryChart"></canvas>
</div>

<script>
    const ctx = document.getElementById('masteryChart').getContext('2d');
    const data = {
        labels: @Html.Raw(JsonSerializer.Serialize(Model.MasteryTrends.Select(m => m.LanguageName))),
        datasets: [{
            label: 'Mastery Level (%)',
            data: @Html.Raw(JsonSerializer.Serialize(Model.MasteryTrends.Select(m => m.MasteryLevel))),
            backgroundColor: 'rgba(102, 126, 234, 0.2)',
            borderColor: 'rgba(102, 126, 234, 1)',
            borderWidth: 2
        }]
    };
    new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            responsive: true,
            scales: {
                y: { beginAtZero: true, max: 100 }
            }
        }
    });
</script>
```

---

## 4️⃣ Data Export Functionality

### **Create Export Service:**

Create `Services/ExportService.cs`:
```csharp
using CsvHelper;
using System.Globalization;
using LinguistPro.Models;

namespace LinguistPro.Services
{
    public class ExportService
    {
        private readonly AppDbContext _db;

        public ExportService(AppDbContext db) => _db = db;

        public async Task<byte[]> ExportVocabularyToCSVAsync(int languageProfileId)
        {
            var items = await _db.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId)
                .ToListAsync();

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<ExportVocabularyDto>();
                await csv.NextRecordAsync();

                foreach (var item in items)
                {
                    csv.WriteRecord(new ExportVocabularyDto
                    {
                        Term = item.Term,
                        Meaning = item.Meaning,
                        Usage = item.Usage,
                        UsageMeaning = item.UsageMeaning,
                        MasteryLevel = item.MasteryLevel
                    });
                    await csv.NextRecordAsync();
                }

                return Encoding.UTF8.GetBytes(writer.ToString());
            }
        }

        public async Task<byte[]> ExportProgressReportAsync(int userId)
        {
            var profiles = await _db.LanguageProfiles
                .Where(lp => lp.UserId == userId)
                .ToListAsync();

            // Generate PDF or JSON report
            // Implementation depends on chosen library
            return new byte[] { };
        }
    }

    public class ExportVocabularyDto
    {
        public string Term { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Usage { get; set; } = string.Empty;
        public string UsageMeaning { get; set; } = string.Empty;
        public int MasteryLevel { get; set; }
    }
}
```

### **Controller/Page Handler:**

```csharp
public async Task<IActionResult> OnGetExportAsync(int languageProfileId)
{
    var export = new ExportService(_db);
    var csv = await export.ExportVocabularyToCSVAsync(languageProfileId);
    
    return File(csv, 
        "text/csv", 
        $"vocabulary_{languageProfileId}_{DateTime.Now:yyyyMMdd}.csv");
}
```

---

## 5️⃣ CSV Import Functionality

### **Create Import Service:**

Create `Services/ImportService.cs`:
```csharp
using CsvHelper;
using System.Globalization;

namespace LinguistPro.Services
{
    public class ImportService
    {
        private readonly AppDbContext _db;

        public ImportService(AppDbContext db) => _db = db;

        public async Task<(int successCount, List<string> errors)> ImportVocabularyAsync(
            Stream fileStream, 
            int languageProfileId)
        {
            var errors = new List<string>();
            int successCount = 0;

            try
            {
                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();

                    int rowNumber = 2;
                    while (await csv.ReadAsync())
                    {
                        try
                        {
                            var record = new ImportVocabularyDto();
                            // Parse CSV fields
                            record.Term = csv.GetField("Term") ?? string.Empty;
                            record.Meaning = csv.GetField("Meaning") ?? string.Empty;
                            record.Usage = csv.GetField("Usage") ?? string.Empty;
                            record.UsageMeaning = csv.GetField("UsageMeaning") ?? string.Empty;

                            if (string.IsNullOrWhiteSpace(record.Term))
                            {
                                errors.Add($"Row {rowNumber}: Term is required");
                                continue;
                            }

                            var vocab = new VocabularyItem
                            {
                                LanguageProfileId = languageProfileId,
                                Term = record.Term,
                                Meaning = record.Meaning,
                                Usage = record.Usage,
                                UsageMeaning = record.UsageMeaning,
                                CreatedDate = DateTime.UtcNow
                            };

                            _db.Vocabulary.Add(vocab);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Row {rowNumber}: {ex.Message}");
                        }

                        rowNumber++;
                    }

                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                errors.Add($"File processing error: {ex.Message}");
            }

            return (successCount, errors);
        }
    }

    public class ImportVocabularyDto
    {
        public string Term { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Usage { get; set; } = string.Empty;
        public string UsageMeaning { get; set; } = string.Empty;
    }
}
```

---

## 📦 Required NuGet Packages

Add these to your `.csproj`:

```xml
<ItemGroup>
    <PackageReference Include="CsvHelper" Version="30.0.0" />
    <PackageReference Include="Chart.js" Version="3.9.1" />
    <PackageReference Include="iTextSharp" Version="5.5.13.3" /> <!-- For PDF export -->
</ItemGroup>
```

Or via Package Manager:
```bash
dotnet add package CsvHelper
dotnet add package Chart.js
dotnet add package iTextSharp
```

---

## 🧪 Testing These Features

### **Unit Test Example:**

Create `Tests/StreakServiceTests.cs`:
```csharp
using Xunit;
using LinguistPro.Services;
using LinguistPro.Models;

public class StreakServiceTests
{
    [Fact]
    public async Task UpdateStreakAsync_IncrementsStreak_WhenActivityTodayIsYesterday()
    {
        // Arrange
        var dbContext = new AppDbContext();
        var service = new StreakService(dbContext);
        var profile = new LanguageProfile { 
            LastActivityDate = DateTime.UtcNow.AddDays(-1),
            CurrentStreak = 5
        };

        // Act
        await service.UpdateStreakAsync(profile.LanguageProfileId);

        // Assert
        Assert.Equal(6, profile.CurrentStreak);
    }
}
```

---

## 🚀 Implementation Order

1. **Learning Streaks** (1-2 days) - Quick win, high engagement
2. **Spaced Repetition** (2-3 days) - Core learning feature
3. **Analytics & Charts** (2-3 days) - Motivational feature
4. **Data Export** (1 day) - Utility feature
5. **CSV Import** (1 day) - Utility feature
6. **Testing** (2-3 days) - Ensure quality

---

## ✅ Success Criteria

- [ ] Streaks display correctly on Dashboard
- [ ] Items marked as correct move to next Leitner box
- [ ] Charts render properly with real data
- [ ] CSV export creates valid file
- [ ] CSV import validates data correctly
- [ ] All unit tests pass
- [ ] No breaking changes to existing functionality

---

**Total Estimated Effort:** 10-15 working days for full Option C implementation

**Recommended Timeline:** 2-3 weeks with testing and refinement

