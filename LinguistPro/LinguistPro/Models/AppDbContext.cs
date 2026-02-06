using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<VocabularyItem> Vocabulary { get; set; }
        public DbSet<VerbEntity> Verbs { get; set; }
    }
}
