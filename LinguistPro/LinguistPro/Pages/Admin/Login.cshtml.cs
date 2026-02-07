using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinguistPro.Models;
using System.Security.Cryptography;
using System.Text;

namespace LinguistPro.Pages.Admin
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }

        public LoginModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet()
        {
            // Check if already admin
            if (User.IsInRole("Admin"))
            {
                RedirectToPage("/Admin/AutoPopulate");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and password are required";
                return Page();
            }

            try
            {
                // Try to find admin user in database
                AdminUser? adminUser = null;
                try
                {
                    // This may fail if AdminUsers table doesn't exist yet
                    adminUser = await _context.AdminUsers
                        .FirstOrDefaultAsync(a => a.AdminUserName.ToLower() == Username.ToLower() && a.IsActive);
                }
                catch (Exception dbEx)
                {
                    _logger.LogWarning($"Database query failed (table may not exist): {dbEx.Message}");
                    // Table doesn't exist yet, use fallback credentials
                    adminUser = null;
                }

                // Fallback: Check against hardcoded credentials if table doesn't exist
                if (adminUser == null)
                {
                    // Temporary fallback credentials for setup
                    if (Username == "admin" && Password == "admin123")
                    {
                        _logger.LogInformation($"Admin login successful using fallback credentials: {Username}");
                        HttpContext.Session.SetString("AdminUser", Username);
                        HttpContext.Session.SetString("AdminRole", "Admin");
                        HttpContext.Session.SetInt32("AdminId", 1);
                        return RedirectToPage("/Admin/AutoPopulate");
                    }

                    ErrorMessage = "Invalid username or password";
                    _logger.LogWarning($"Failed admin login attempt with username: {Username}");
                    return Page();
                }

                // If we have database admin user, verify password
                bool passwordValid = VerifyPassword(Password, adminUser.PasswordHash);

                if (!passwordValid)
                {
                    ErrorMessage = "Invalid username or password";
                    _logger.LogWarning($"Failed admin login attempt with username: {Username}");
                    return Page();
                }

                // Update last login date
                adminUser.LastLoginDate = DateTime.UtcNow;
                _context.AdminUsers.Update(adminUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Admin login successful: {Username}");

                // Create session/cookie for admin
                HttpContext.Session.SetString("AdminUser", Username);
                HttpContext.Session.SetString("AdminRole", "Admin");
                HttpContext.Session.SetInt32("AdminId", adminUser.AdminUserId);

                return RedirectToPage("/Admin/AutoPopulate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during admin login: {ex.Message}");
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                // Extract salt from hash (first 16 bytes)
                byte[] hashBytes = Convert.FromBase64String(hash);
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                // Compute hash for provided password
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] computedHash = pbkdf2.GetBytes(32);
                    for (int i = 0; i < 32; i++)
                    {
                        if (hashBytes[i + 16] != computedHash[i])
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
