using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LinguistPro.Models;
using System.ComponentModel.DataAnnotations;

namespace LinguistPro.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public RegisterModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    DateOfBirth = Input.DateOfBirth,
                    Country = Input.Country,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Create a UserProfile for the new user
                    var userProfile = new UserProfile
                    {
                        UserId = user.Id,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        DateOfBirth = Input.DateOfBirth,
                        Country = Input.Country,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    _db.UserProfiles.Add(userProfile);
                    await _db.SaveChangesAsync();

                    // Create default language profiles for the user
                    var defaultLanguages = new[] 
                    { 
                        new { Code = "de", Name = "German" },
                        new { Code = "fr", Name = "French" },
                        new { Code = "es", Name = "Spanish" },
                        new { Code = "ru", Name = "Russian" }
                    };

                    foreach (var lang in defaultLanguages)
                    {
                        var langProfile = new LanguageProfile
                        {
                            UserId = user.Id,
                            LanguageCode = lang.Code,
                            LanguageName = lang.Name,
                            IsActive = lang.Code == "de", // German is the default
                            CreatedDate = DateTime.UtcNow,
                            MasteryLevel = 0
                        };
                        _db.LanguageProfiles.Add(langProfile);
                    }

                    await _db.SaveChangesAsync();

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        public class RegisterInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords don't match")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;

            public DateTime? DateOfBirth { get; set; }

            [StringLength(100)]
            public string Country { get; set; } = string.Empty;
        }
    }
}
