using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Model
{
    public class ChangePassword
    {
        [Required, DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required"), DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,50}$",
            ErrorMessage = "Password must be at least 12 characters long and contain at least 1 lowercase, 1 uppercase, 1 digit, and 1 special character.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
