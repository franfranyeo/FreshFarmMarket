using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FreshFarmMarket.Model;

namespace FreshFarmMarket.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> _userManager;
        //private readonly AuditLogServices _auditLogService;
        //private readonly SmsService _smsService;

        public LoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager
            //AuditLogServices auditLogService, 
            //SmsService smsService, 
            //GooglereCaptchaService _googleCaptchaService
            )
        {
            this.signInManager = signInManager;
            _userManager = userManager;
            //_auditLogService = auditLogService;
            //_smsService = smsService;
        }
        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            // Google recaptcha
            //GoogleResponse _GooglereCaptcha = await _GooglereCaptchaService.VerifyCaptcha(LModel.Token);
            //if (!_GooglereCaptcha.success && _GooglereCaptcha.score <= 0.5)
            //{
            //	ModelState.AddModelError("", "You are not human!");
            //	return Page();
            //}

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(LModel.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, LModel.Password))
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return Page();
            }
            else
            {
                // Check if user is already logged in
                if (user.IsLoggedIn)
                {
                    ModelState.AddModelError("", "User is already logged in from another device.");
                    return Page();
                }

                var result = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {// Check maximum password age
                    if (user.LastPasswordChange.HasValue &&
                        DateTime.Now > user.LastPasswordChange.Value.AddDays(180)) // 6 months 
                    {
                        // Redirect to change password page
                        return RedirectToPage("ChangePassword");
                    }

                    user.IsLoggedIn = true;
                    await _userManager.UpdateAsync(user);
                    //await _auditLogService.RecordLogs(Actions.Login, LModel.Email);
                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = "You have successfully logged in.";

                    HttpContext.Session.SetString("SessionEmail", LModel.Email);
                    return RedirectToPage("Index");
                }
                else if (result.IsLockedOut)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = "Your account have been locked out";
                    return Page();
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("TwoFactorAuth");
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = "Invalid username or password";
                    return Page();
                }

            }
        }
    }
}
