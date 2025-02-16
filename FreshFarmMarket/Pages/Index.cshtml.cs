using FreshFarmMarket.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _http;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(IHttpContextAccessor http, UserManager<User> userManager, IDataProtectionProvider provider, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("CreditCardProtector");
            _http = http;
            _signInManager = signInManager;
        }

        public User FetchUser { get; set; } = new User();

        public string? Username { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Username = HttpContext.Session.GetString("SessionEmail");
            //if (Username == null)
            //{
            //    return RedirectToPage("Login");
            //}

            if (string.IsNullOrEmpty(Username))
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear();
                return RedirectToPage("Login");
            }

            var user = await _userManager.FindByEmailAsync(Username);
            if (user == null)
            {
                return RedirectToPage("Login");
            }
            var decryptedCreditCardNo = _protector.Unprotect(user.CreditCardNo);
            user.CreditCardNo = decryptedCreditCardNo;
            FetchUser = user;

            return Page();
        }
    }
}
