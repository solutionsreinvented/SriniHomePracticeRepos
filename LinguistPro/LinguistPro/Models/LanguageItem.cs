using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    /// <summary>
    /// Base class for language learning items (Numbers, Months, Days)
    /// </summary>
    public class LanguageItem
    {
        [Key]
        public int Id { get; set; }

        public required string Language { get; set; }
        public required string ItemType { get; set; } // "Number", "Month", "Day"
        public required string Term { get; set; }
        public string Meaning { get; set; } = string.Empty;
        public string UsageExample { get; set; } = string.Empty;
        public string UsageExampleMeaning { get; set; } = string.Empty;

        public int Mastery { get; set; } = 0;
        public DateTime LastReviewed { get; set; } = DateTime.UtcNow;
    }
}
