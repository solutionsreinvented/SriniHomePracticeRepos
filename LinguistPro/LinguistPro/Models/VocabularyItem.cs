using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    public class VocabularyItem
    {
        [Key] public int Id { get; set; }
        public required string SourceWord { get; set; }
        public required string TargetMeaning { get; set; }
        public required string Definition { get; set; }
        public required string UsageExample { get; set; }
        public int MasteryPoints { get; set; }
        public DateTime LastInteraction { get; set; } = DateTime.Now;
    }
}
