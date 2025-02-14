using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> _userManager;
        private readonly LogService _logService;
        public LogoutModel(SignInManager<User> signInManager,  UserManager<User> userManager, LogService logService)
        {
            this.signInManager = signInManager;
            _userManager = userManager;
            _logService = logService;
        }
        public void OnGet() { }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.IsLoggedIn = false;
                await _userManager.UpdateAsync(user);
                await _logService.RecordLogs("Logout", user.Email);
                HttpContext.Session.Clear();
            }
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
