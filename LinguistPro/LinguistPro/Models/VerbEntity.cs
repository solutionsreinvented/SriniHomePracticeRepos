using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    public class VerbEntity
    {
        [Key] public int Id { get; set; }
        public required string Infinitive { get; set; }
        public required string Meaning { get; set; }

        // Agnostic Conjugation Schema
        public required string S1 { get; set; }  // 1st Singular
        public required string S2 { get; set; }  // 2nd Singular (Informal)
        public required string S2F { get; set; } // 2nd Singular (Formal)
        public required string S3 { get; set; }  // 3rd Singular

        public required string P1 { get; set; }  // 1st Plural
        public required string P2 { get; set; }  // 2nd Plural (Informal)
        public required string P2F { get; set; } // 2nd Plural (Formal)
        public required string P3 { get; set; }  // 3rd Plural
    }
}
