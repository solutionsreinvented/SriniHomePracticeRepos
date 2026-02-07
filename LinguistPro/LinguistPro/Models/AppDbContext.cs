using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VocabularyItem> Vocabulary => Set<VocabularyItem>();
        public DbSet<VerbEntry> Verbs => Set<VerbEntry>();
        public DbSet<LanguageItem> LanguageItems => Set<LanguageItem>();
    }
}
