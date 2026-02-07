using Microsoft.AspNetCore.Identity;

namespace LinguistPro.Models
{
    /// <summary>
    /// Extended user class for ASP.NET Identity with additional profile information
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        public override string ToString()
        {
            return $"{UserName} ({FirstName} {LastName})";
        }
    }
}
