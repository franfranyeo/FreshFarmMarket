using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Model
{
    public class TwoFactorAuth
    {
        [Required]
        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; }
    }
}
