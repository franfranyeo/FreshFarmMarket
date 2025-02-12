
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using FreshFarmMarket.Model;
using FreshFarmMarket.Services;

namespace FreshFarmMarket.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        // Inject EmailSender or similar service to send emails
        private readonly EmailService _emailService;

        public ForgotPasswordModel(UserManager<User> userManager, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    // Generate the reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Send email with reset link
                    var resetLink = Url.Page("/ResetPassword", null, new { token = Uri.EscapeDataString(token), email = Uri.EscapeDataString(user.Email) }, Request.Scheme);
                    // Use your email service to send the link
                    var emailContent = System.Net.WebUtility.HtmlEncode($"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");
                    await _emailService.SendEmailAsync(user.Email, "Reset Password", emailContent);
                }
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "Password reset link has been sent to your email if it exists.";


            }
            // Handle invalid or non-existing email
            return Page();
        }
    }

}
