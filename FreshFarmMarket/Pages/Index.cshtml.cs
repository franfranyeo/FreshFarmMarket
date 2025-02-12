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

        public IndexModel(IHttpContextAccessor http, UserManager<User> userManager, IDataProtectionProvider provider)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("CreditCardProtector");
            _http = http;
        }

        public User FetchUser { get; set; } = new User();

        public string? Username { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Username = HttpContext.Session.GetString("SessionEmail");
            if (Username == null)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(Username);
            var decryptedCreditCardNo = _protector.Unprotect(user.CreditCardNo);
            user.CreditCardNo = decryptedCreditCardNo;
            FetchUser = user;

            return Page();
        }
    }
}
