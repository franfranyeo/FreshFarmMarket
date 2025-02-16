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
        private readonly LogService _logService;


        public TwoFactorAuthModel(SignInManager<User> signInManager, SmsService smsService, UserManager<User> userManager, LogService logService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _smsService = smsService;
            _logService = logService;
        }

        [BindProperty]
        public TwoFactorAuth TFAModel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
            await _smsService.SendSms(user.MobileNo, token);

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
                user.IsLoggedIn = true;
                await _userManager.UpdateAsync(user);

                await _logService.RecordLogs("Login (2FA)", user.Email);

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
