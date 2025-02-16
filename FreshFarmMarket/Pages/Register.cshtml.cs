using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IWebHostEnvironment _environment;
        private readonly IDataProtector _protector;
        private readonly LogService _logService;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IDataProtectionProvider provider,
            IWebHostEnvironment environment,
                LogService logService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _protector = provider.CreateProtector("CreditCardProtector");
            _environment = environment;
            _logService = logService;
        }

        [BindProperty]
        public User RModel { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,50}$",
            ErrorMessage = "Password must be at least 12 characters long and contain at least 1 lowercase, 1 uppercase, 1 digit, and 1 special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Profile Photo is required")]
        public IFormFile? ImageUpload { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
                return Page();

            // Validate if Email is already taken
            var existingUser = await _userManager.FindByEmailAsync(Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return Page();
            }

            if (ImageUpload == null)
            {
                ModelState.AddModelError("ImageUpload", "Please upload a photo.");
                return Page();
            }

            var fileExtension = Path.GetExtension(ImageUpload.FileName).ToLower();
            if (fileExtension != ".jpg")
            {
                ModelState.AddModelError("ImageUpload", "Only .JPG files are allowed.");
                return Page();
            }

            if (ImageUpload.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                ModelState.AddModelError("ImageUpload", "File size cannot exceed 5MB.");
                return Page();
            }

            var uploadsFolder = "uploads";
            var originalFileNameWithoutExtension = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
            var filename = $"{Guid.NewGuid()}_{originalFileNameWithoutExtension}{fileExtension}";
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, filename);
            var photoURL = string.Format("/{0}/{1}", uploadsFolder, filename);

            var protectedCreditCardNo = _protector.Protect(RModel.CreditCardNo);


            RModel.DeliveryAddress = HttpUtility.HtmlEncode(RModel.DeliveryAddress);
            RModel.AboutMe = HttpUtility.HtmlEncode(RModel.AboutMe);


            var user = new User
            {
                FullName = RModel.FullName,
                Email = Email,
                UserName = Email,
                CreditCardNo = protectedCreditCardNo,
                Gender = RModel.Gender,
                MobileNo = "+65" + RModel.MobileNo,
                DeliveryAddress = RModel.DeliveryAddress,
                AboutMe = RModel.AboutMe,
                Photo = photoURL,
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByEmailAsync(Email);
                if (currentUser != null) {
                    currentUser.EmailConfirmed = true;
                    currentUser.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(currentUser);
                }

                if (Email == "admin@freshfarmmarket.com")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await ImageUpload.CopyToAsync(fileStream);
                await _signInManager.SignInAsync(user, isPersistent: false);
                await _logService.RecordLogs("Login", Email);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "You have successfully registered for an account";
                HttpContext.Session.SetString("SessionEmail", Email);
                await _userManager.SetPhoneNumberAsync(user, user.MobileNo);
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
