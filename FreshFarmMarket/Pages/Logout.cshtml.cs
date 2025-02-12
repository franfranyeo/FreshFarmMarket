using FreshFarmMarket.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> _userManager;
        public LogoutModel(SignInManager<User> signInManager,  UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            _userManager = userManager;
        }
        public void OnGet() { }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.IsLoggedIn = false;
                await _userManager.UpdateAsync(user);
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
