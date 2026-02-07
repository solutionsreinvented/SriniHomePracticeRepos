namespace LinguistPro.Models
{
    /// <summary>
    /// Secure admin user management
    /// Stores admin credentials separately from regular users
    /// </summary>
    public class AdminUser
    {
        public int AdminUserId { get; set; }
        public required string AdminUserName { get; set; }
        public required string PasswordHash { get; set; }  // BCrypt hashed
        public string? FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
    }
}
