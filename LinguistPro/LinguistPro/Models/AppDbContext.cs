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
        }
    }
}
