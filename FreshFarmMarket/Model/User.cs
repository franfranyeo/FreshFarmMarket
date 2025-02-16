using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace FreshFarmMarket.Model
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Full Name is required")]
        [DataType(DataType.Text)]
        [MaxLength(20, ErrorMessage = "Full Name cannot be longer than 20 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required"), StringLength(255)]
        [MinLength(13, ErrorMessage = "Credit Card Number must be between 13 to 19 digits.")]
        [MaxLength(19, ErrorMessage = "Credit Card Number must be between 13 to 19 digits.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Credit Card Number can only contain numbers.")]
        public string CreditCardNo { get; set; }

        [Required(ErrorMessage = "Gender is required"), StringLength(10)]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone Number is required"), StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{8,}$", ErrorMessage = "Mobile number must be at least 8 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Delivery Address is required")]
        [MinLength(10, ErrorMessage = "Delivery Address must be at least 10 characters long.")]
        [MaxLength(150, ErrorMessage = "Delivery Address cannot be longer than 150 characters")]
        public string DeliveryAddress { get; set; }

        [MaxLength(150)]
        public string? Photo { get; set; }

        [Required(ErrorMessage = "About Me is required")]
        [MaxLength(150, ErrorMessage = "About Me cannot be longer than 150 characters")]
        public string AboutMe { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public string? PreviousPWHash { get; set; }
        public string? PreviousPWHash2 { get; set; }
    }
}
