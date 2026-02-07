using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinguistPro.Models;

namespace LinguistPro.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public ProfileModel(UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public string CurrentUserEmail { get; set; } = string.Empty;
        public UserProfile? UserProfile { get; set; }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string Country { get; set; } = string.Empty;

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            CurrentUserEmail = user.Email ?? string.Empty;

            UserProfile = await _db.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == user.Id);

            if (UserProfile != null)
            {
                FirstName = UserProfile.FirstName;
                LastName = UserProfile.LastName;
                Country = UserProfile.Country;
                DateOfBirth = UserProfile.DateOfBirth ?? DateTime.MinValue;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            CurrentUserEmail = user.Email ?? string.Empty;

            UserProfile = await _db.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == user.Id);

            if (UserProfile == null)
            {
                ErrorMessage = "User profile not found.";
                return Page();
            }

            try
            {
                // Update ApplicationUser names
                user.FirstName = FirstName.Trim();
                user.LastName = LastName.Trim();
                await _userManager.UpdateAsync(user);

                // Update UserProfile
                UserProfile.FirstName = FirstName.Trim();
                UserProfile.LastName = LastName.Trim();
                UserProfile.Country = Country.Trim();
                UserProfile.DateOfBirth = DateOfBirth;

                _db.UserProfiles.Update(UserProfile);
                await _db.SaveChangesAsync();

                SuccessMessage = "Profile updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return Page();
        }
    }
}
