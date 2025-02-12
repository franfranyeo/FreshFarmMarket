using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class TwoFactorAuthModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly SmsService _smsService;
        private readonly UserManager<User> _userManager;


        public TwoFactorAuthModel(SignInManager<User> signInManager, SmsService smsService, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _smsService = smsService;
        }

        [BindProperty]
        public TwoFactorAuth TFAModel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
            await _smsService.SendSms(user.PhoneNumber, token);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            var result = await _signInManager.TwoFactorSignInAsync("Phone", TFAModel.VerificationCode, false, false);

            if (result.Succeeded)
            {
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "You have successfully logged in.";
                HttpContext.Session.SetString("SessionEmail", user.Email);
                return RedirectToPage("Index");
            }
            else if (result.IsLockedOut)
            {
                // Handle lockout scenario
                return RedirectToPage("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
                return Page();
            }
        }
    }
}
