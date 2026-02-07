using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Models
{
    /// <summary>
    /// Represents a user profile in the application.
    /// Each user can have multiple language profiles.
    /// </summary>
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [StringLength(255)]
        public string EmailAddress { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<LanguageProfile> LanguageProfiles { get; set; } = new List<LanguageProfile>();

        public override string ToString()
        {
            return $"{Username} ({FirstName} {LastName})";
        }
    }
}
