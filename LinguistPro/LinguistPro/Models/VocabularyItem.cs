using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    public class VocabularyItem
    {
        [Key]
        public int Id { get; set; }

        public required string Language { get; set; }
        public required string Term { get; set; }

        public string Meaning { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string UsageExample { get; set; } = string.Empty;

        public int Mastery { get; set; } = 0;
        public DateTime LastReviewed { get; set; } = DateTime.UtcNow;
    }
}
