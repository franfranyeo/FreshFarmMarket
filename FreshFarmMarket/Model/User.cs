using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace FreshFarmMarket.Model
{
    public class User : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "Full Name cannot be longer than 20 characters")]
        public string FullName { get; set; }

        [Required, StringLength(255)]
        [DataType(DataType.CreditCard)]
        public string CreditCardNo { get; set; }

        [Required, StringLength(10)]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required, StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{8,}$", ErrorMessage = "Mobile number must be at least 8 digits.")]
        public string MobileNo { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [MaxLength(150)]
        public string? Photo { get; set; }

        public bool TwoFactorEnabled { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "About Me cannot be longer than 150 characters")]
        public string AboutMe { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public string? PreviousPWHash { get; set; }
        public string? PreviousPWHash2 { get; set; }
    }
}
