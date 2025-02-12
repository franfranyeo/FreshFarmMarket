using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FreshFarmMarket.Model;

namespace FreshFarmMarket.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ChangePassword ChangePassModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Minimum password age check (e.g., 60 minutes)
            if (user.LastPasswordChange.HasValue && DateTime.Now < user.LastPasswordChange.Value.AddMinutes(60))
            {
                ModelState.AddModelError("", "You cannot change your password yet.");
                return Page();
            }

            var passwordHasher = _userManager.PasswordHasher;

            bool isPasswordReused = false;
            if (user.PreviousPWHash != null)
            {
                PasswordVerificationResult passwordMatch = passwordHasher.VerifyHashedPassword(user, user.PreviousPWHash, ChangePassModel.NewPassword);
                if (passwordMatch == PasswordVerificationResult.Success)
                {
                    isPasswordReused = true;
                }
            }
            if (user.PreviousPWHash2 != null)
            {
                PasswordVerificationResult passwordMatch1 = passwordHasher.VerifyHashedPassword(user, user.PreviousPWHash2, ChangePassModel.NewPassword);
                if (passwordMatch1 == PasswordVerificationResult.Success)
                {
                    isPasswordReused = true;
                }
            }

            if (isPasswordReused)
            {
                ModelState.AddModelError("", "You cannot reuse a previous password.");
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, ChangePassModel.OldPassword, ChangePassModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            user.PreviousPWHash2 = user.PreviousPWHash;
            user.PreviousPWHash = user.PasswordHash;
            user.LastPasswordChange = DateTime.Now;
            await _userManager.UpdateAsync(user);

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "Password reset link has been sent to your email if it exists.";

            return RedirectToPage("Index");
        }
    }
}
