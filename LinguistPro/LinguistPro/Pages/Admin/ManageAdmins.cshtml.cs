using LinguistPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LinguistPro.Pages.Admin
{
    public class ManageAdminsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ManageAdminsModel> _logger;

        public List<AdminUserViewModel> AdminUsers { get; set; } = new();
        public bool IsSuccess { get; set; }
        public bool HasError { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsAdminAuthenticated { get; set; }

        [BindProperty]
        public CreateAdminModel CreateAdmin { get; set; } = new();

        public class CreateAdminModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
            public string? ConfirmPassword { get; set; }
            public string? FullName { get; set; }
        }

        public class AdminUserViewModel
        {
            public string? Id { get; set; }
            public string? Username { get; set; }
            public string? FullName { get; set; }
            public DateTime CreatedDate { get; set; }
            public int? AdminUserId { get; set; }
        }

        public ManageAdminsModel(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ManageAdminsModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated via admin session
            var adminUser = HttpContext.Session.GetString("AdminUser");
            if (string.IsNullOrEmpty(adminUser))
            {
                _logger.LogWarning("Unauthorized access attempt to Manage Admins page");
                return RedirectToPage("/Admin/Login");
            }

            IsAdminAuthenticated = true;

            // Load all admin users - with graceful fallback if table doesn't exist
            try
            {
                var admins = await _context.AdminUsers.ToListAsync();

                AdminUsers = admins.Select(a => new AdminUserViewModel
                {
                    Id = a.AdminUserId.ToString(),
                    Username = a.AdminUserName,
                    FullName = a.FullName,
                    CreatedDate = a.CreatedDate,
                    AdminUserId = a.AdminUserId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"AdminUsers table may not exist yet: {ex.Message}");
                AdminUsers = new List<AdminUserViewModel>();
                ErrorMessage = "⚠️ Admin database table not initialized. Run migrations first: dotnet ef migrations add AddAdminUsers && dotnet ef database update";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            // Security check
            var adminUser = HttpContext.Session.GetString("AdminUser");
            if (string.IsNullOrEmpty(adminUser))
            {
                HasError = true;
                ErrorMessage = "Unauthorized access";
                return RedirectToPage("/Admin/Login");
            }

            IsAdminAuthenticated = true;

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(CreateAdmin.Username))
                {
                    HasError = true;
                    ErrorMessage = "Username is required";
                    await ReloadAdminsList();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(CreateAdmin.Password) || CreateAdmin.Password.Length < 6)
                {
                    HasError = true;
                    ErrorMessage = "Password must be at least 6 characters long";
                    await ReloadAdminsList();
                    return Page();
                }

                if (CreateAdmin.Password != CreateAdmin.ConfirmPassword)
                {
                    HasError = true;
                    ErrorMessage = "Passwords do not match";
                    await ReloadAdminsList();
                    return Page();
                }

                // Check if username already exists
                try
                {
                    var existing = await _context.AdminUsers
                        .FirstOrDefaultAsync(a => a.AdminUserName.ToLower() == CreateAdmin.Username.ToLower());

                    if (existing != null)
                    {
                        HasError = true;
                        ErrorMessage = "Username already exists";
                        await ReloadAdminsList();
                        return Page();
                    }
                }
                catch (Exception dbEx)
                {
                    _logger.LogWarning($"AdminUsers table may not exist: {dbEx.Message}");
                    HasError = true;
                    ErrorMessage = "⚠️ Admin database not initialized. Run migrations first.";
                    return Page();
                }

                // Hash password (use PBKDF2 for security)
                var hashedPassword = HashPassword(CreateAdmin.Password);

                // Create new admin user
                var newAdmin = new AdminUser
                {
                    AdminUserName = CreateAdmin.Username,
                    PasswordHash = hashedPassword,
                    FullName = CreateAdmin.FullName,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _context.AdminUsers.Add(newAdmin);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"[ADMIN: {adminUser}] Created new admin user: {CreateAdmin.Username}");

                IsSuccess = true;
                SuccessMessage = $"✅ Admin user '{CreateAdmin.Username}' created successfully!";

                // Clear form
                CreateAdmin = new CreateAdminModel();

                await ReloadAdminsList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating admin user: {ex.Message}");
                HasError = true;
                ErrorMessage = $"Error: {ex.Message}";
                await ReloadAdminsList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // Security check
            var adminUser = HttpContext.Session.GetString("AdminUser");
            if (string.IsNullOrEmpty(adminUser))
            {
                HasError = true;
                ErrorMessage = "Unauthorized access";
                return RedirectToPage("/Admin/Login");
            }

            IsAdminAuthenticated = true;

            try
            {
                var adminToDelete = await _context.AdminUsers
                    .FirstOrDefaultAsync(a => a.AdminUserId == id);

                if (adminToDelete == null)
                {
                    HasError = true;
                    ErrorMessage = "Admin user not found";
                    await ReloadAdminsList();
                    return Page();
                }

                _context.AdminUsers.Remove(adminToDelete);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"[ADMIN: {adminUser}] Deleted admin user: {adminToDelete.AdminUserName}");

                IsSuccess = true;
                SuccessMessage = $"✅ Admin user '{adminToDelete.AdminUserName}' deleted successfully!";

                await ReloadAdminsList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting admin user: {ex.Message}");
                HasError = true;
                ErrorMessage = $"Error: {ex.Message}";
                await ReloadAdminsList();
            }

            return Page();
        }

        private async Task ReloadAdminsList()
        {
            try
            {
                var admins = await _context.AdminUsers.ToListAsync();

                AdminUsers = admins.Select(a => new AdminUserViewModel
                {
                    Id = a.AdminUserId.ToString(),
                    Username = a.AdminUserName,
                    FullName = a.FullName,
                    CreatedDate = a.CreatedDate,
                    AdminUserId = a.AdminUserId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not reload admin list: {ex.Message}");
                AdminUsers = new List<AdminUserViewModel>();
            }
        }

        private string HashPassword(string password)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
            {
                byte[] salt = pbkdf2.Salt;
                byte[] hash = pbkdf2.GetBytes(32);
                byte[] hashWithSalt = new byte[48];
                Array.Copy(salt, 0, hashWithSalt, 0, 16);
                Array.Copy(hash, 0, hashWithSalt, 16, 32);
                return Convert.ToBase64String(hashWithSalt);
            }
        }
    }
}
