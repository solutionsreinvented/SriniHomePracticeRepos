using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinguistPro.Pages.Admin
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }

        public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _configuration = configuration;
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

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and password are required";
                return Page();
            }

            // Get admin credentials from configuration
            var adminUsername = _configuration["Admin:Username"] ?? "admin";
            var adminPassword = _configuration["Admin:Password"] ?? "admin123";

            // Validate credentials (in production, hash passwords!)
            if (Username == adminUsername && Password == adminPassword)
            {
                _logger.LogInformation($"Admin login successful: {Username}");
                
                // Create session/cookie for admin
                HttpContext.Session.SetString("AdminUser", Username);
                HttpContext.Session.SetString("AdminRole", "Admin");
                
                return RedirectToPage("/Admin/AutoPopulate");
            }
            else
            {
                ErrorMessage = "Invalid username or password";
                _logger.LogWarning($"Failed admin login attempt with username: {Username}");
                return Page();
            }
        }
    }
}
