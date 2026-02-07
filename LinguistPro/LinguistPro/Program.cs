using LinguistPro.Models;
using LinguistPro.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add database context
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=linguist.db"));

// Add ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add services
builder.Services.AddHttpClient<DictionaryService>();
builder.Services.AddHttpClient<WiktionaryVerbService>();
builder.Services.AddHttpClient<PronunciationFetcherService>();
builder.Services.AddHttpClient<VocabularyAutoFetcherService>();
builder.Services.AddHttpClient<LanguageDataAutoPopulatorService>();
builder.Services.AddScoped<LearningStreakService>();
builder.Services.AddScoped<SpacedRepetitionService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<UserPreferencesService>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<PronunciationService>();
builder.Services.AddScoped<PronunciationFetcherService>();
builder.Services.AddScoped<VocabularyAutoFetcherService>();
builder.Services.AddScoped<LanguageDataAutoPopulatorService>();
builder.Services.AddScoped<SearchFilterService>();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
});

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed test streak data if needed (only in development)
    if (app.Environment.IsDevelopment())
    {
        await SeedTestStreakDataAsync(db);
    }
}

app.UseStaticFiles();
app.UseRouting();

// Add authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

// Helper function to seed test streak data
async Task SeedTestStreakDataAsync(AppDbContext db)
{
    try
    {
        // Find a language profile (get the first one or create test data)
        var languageProfile = await db.LanguageProfiles.FirstOrDefaultAsync();
        if (languageProfile == null)
            return; // No profiles yet

        // Check if streak already exists
        var existingStreak = await db.LearningStreaks.FirstOrDefaultAsync(s => s.LanguageProfileId == languageProfile.LanguageProfileId);
        if (existingStreak != null)
            return; // Already seeded

        // Create a test streak with 7 days
        var streak = new LearningStreak
        {
            LanguageProfileId = languageProfile.LanguageProfileId,
            CurrentStreak = 7,
            LongestStreak = 7,
            StreakStartDate = DateTime.UtcNow.AddDays(-6),
            LastLearningDate = DateTime.UtcNow,
            TotalLearningDays = 7,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };

        db.LearningStreaks.Add(streak);

        // Add some test daily logs
        for (int i = 0; i < 7; i++)
        {
            var log = new DailyLearningLog
            {
                LanguageProfileId = languageProfile.LanguageProfileId,
                LearningDate = DateTime.UtcNow.AddDays(-6 + i),
                VocabularyItemsLearned = 3 + i,
                VerbsLearned = 1,
                LanguageItemsLearned = 2,
                TotalTimeSpentSeconds = 1800 + (i * 300),
                ItemsMastered = i > 4 ? 1 : 0,
                MasteryLevelAtEndOfDay = 20 + (i * 5),
                CreatedDate = DateTime.UtcNow.AddDays(-6 + i),
                LastUpdatedDate = DateTime.UtcNow.AddDays(-6 + i)
            };
            db.DailyLearningLogs.Add(log);
        }

        await db.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Silently fail - don't break the app if seeding fails
        System.Diagnostics.Debug.WriteLine($"Seeding failed: {ex.Message}");
    }
}
