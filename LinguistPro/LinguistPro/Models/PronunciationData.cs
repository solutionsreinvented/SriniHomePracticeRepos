using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    /// <summary>
    /// IPA pronunciation data for vocabulary and language items
    /// </summary>
    public class PronunciationData
    {
        [Key]
        public int PronunciationId { get; set; }

        /// <summary>
        /// IPA phonetic transcription (e.g., /kæt/ for English "cat")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string IPA { get; set; } = string.Empty;

        /// <summary>
        /// Audio file URL for pronunciation (stored in wwwroot/audio/pronunciations/)
        /// </summary>
        [StringLength(500)]
        public string? AudioUrl { get; set; }

        /// <summary>
        /// Pronunciation difficulty (Beginner, Intermediate, Advanced)
        /// </summary>
        [StringLength(50)]
        public string DifficultySyllables { get; set; } = "simple";

        /// <summary>
        /// Phonetic breakdown by syllables (e.g., "CAT" or "CON-TIN-UE")
        /// </summary>
        [StringLength(200)]
        public string? SyllableBreakdown { get; set; }

        /// <summary>
        /// Notes about pronunciation (e.g., "roll the R", "nasal sound")
        /// </summary>
        [StringLength(500)]
        public string? PronunciationNotes { get; set; }

        /// <summary>
        /// Language code (de, fr, es, ru, ko)
        /// </summary>
        [Required]
        [StringLength(5)]
        public string LanguageCode { get; set; } = string.Empty;

        /// <summary>
        /// Word/term this pronunciation applies to
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Word { get; set; } = string.Empty;

        /// <summary>
        /// When pronunciation was added/updated
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// How many times users have listened to this pronunciation
        /// </summary>
        public int PlayCount { get; set; } = 0;
    }
}
