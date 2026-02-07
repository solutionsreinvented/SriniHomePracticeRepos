using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Represents a user's language learning profile.
    /// Each user can learn multiple languages simultaneously.
    /// Each language has its own vocabulary, verbs, and other learning items.
    /// </summary>
    public class LanguageProfile
    {
        [Key]
        public int LanguageProfileId { get; set; }

        [Required]
        [ForeignKey(nameof(UserProfile))]
        public int UserId { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = string.Empty; // "de", "fr", "es", etc.

        [Required]
        [StringLength(100)]
        public string LanguageName { get; set; } = string.Empty; // "German", "French", "Spanish", etc.

        public int MasteryLevel { get; set; } = 0; // Overall mastery level (0-100)

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public UserProfile? UserProfile { get; set; }

        public ICollection<VocabularyItem> VocabularyItems { get; set; } = new List<VocabularyItem>();

        public ICollection<VerbEntry> VerbEntries { get; set; } = new List<VerbEntry>();

        public ICollection<LanguageItem> LanguageItems { get; set; } = new List<LanguageItem>();

        public override string ToString()
        {
            return $"{LanguageName} (User: {UserId})";
        }
    }
}
