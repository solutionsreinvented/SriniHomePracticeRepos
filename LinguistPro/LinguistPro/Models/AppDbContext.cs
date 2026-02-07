using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VocabularyItem> Vocabulary => Set<VocabularyItem>();
        public DbSet<VerbEntry> Verbs => Set<VerbEntry>();
        public DbSet<LanguageItem> LanguageItems => Set<LanguageItem>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<LanguageProfile> LanguageProfiles => Set<LanguageProfile>();
        public DbSet<LearningStreak> LearningStreaks => Set<LearningStreak>();
        public DbSet<DailyLearningLog> DailyLearningLogs => Set<DailyLearningLog>();
        public DbSet<ReviewSchedule> ReviewSchedules => Set<ReviewSchedule>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<UserProfile>()
                .HasKey(u => u.UserId);

            builder.Entity<UserProfile>()
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserProfile>()
                .HasMany(u => u.LanguageProfiles)
                .WithOne(l => l.UserProfile)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LanguageProfile>()
                .HasMany(l => l.VocabularyItems)
                .WithOne(v => v.LanguageProfile)
                .HasForeignKey(v => v.LanguageProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LanguageProfile>()
                .HasMany(l => l.VerbEntries)
                .WithOne(v => v.LanguageProfile)
                .HasForeignKey(v => v.LanguageProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LanguageProfile>()
                .HasMany(l => l.LanguageItems)
                .WithOne(i => i.LanguageProfile)
                .HasForeignKey(i => i.LanguageProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Learning Streak relationship
            builder.Entity<LanguageProfile>()
                .HasOne(l => l.LearningStreak)
                .WithOne(s => s.LanguageProfile)
                .HasForeignKey<LearningStreak>(s => s.LanguageProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Daily Learning Log relationship
            builder.Entity<LanguageProfile>()
                .HasMany(l => l.DailyLearningLogs)
                .WithOne(d => d.LanguageProfile)
                .HasForeignKey(d => d.LanguageProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review Schedule relationships for Vocabulary
            builder.Entity<ReviewSchedule>()
                .HasOne(r => r.VocabularyItem)
                .WithOne(v => v.ReviewSchedule)
                .HasForeignKey<ReviewSchedule>(r => r.VocabularyItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review Schedule relationships for Language Items
            builder.Entity<ReviewSchedule>()
                .HasOne(r => r.LanguageItem)
                .WithOne(l => l.ReviewSchedule)
                .HasForeignKey<ReviewSchedule>(r => r.LanguageItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for efficient querying
            builder.Entity<ReviewSchedule>()
                .HasIndex(r => r.NextReviewDate);

            builder.Entity<ReviewSchedule>()
                .HasIndex(r => r.LeitnerBox);


            // Index for efficient querying of daily logs
            builder.Entity<DailyLearningLog>()
                .HasIndex(d => new { d.LanguageProfileId, d.LearningDate })
                .IsUnique();
        }
    }
}
