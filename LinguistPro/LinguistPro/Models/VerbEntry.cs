using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    public class VerbEntry
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        public required string Language { get; set; }
        public required string Infinitive { get; set; }
        public string Meaning { get; set; } = string.Empty;

        public string Tense { get; set; } = "Present";

        // Language-agnostic conjugation slots
        public string S1 { get; set; } = string.Empty;
        public string S2Inf { get; set; } = string.Empty;
        public string S2Form { get; set; } = string.Empty;
        public string S3 { get; set; } = string.Empty;

        public string P1 { get; set; } = string.Empty;
        public string P2Inf { get; set; } = string.Empty;
        public string P2Form { get; set; } = string.Empty;
        public string P3 { get; set; } = string.Empty;

        // Navigation property
        public LanguageProfile? LanguageProfile { get; set; }
    }
}
