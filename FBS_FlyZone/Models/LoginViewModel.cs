using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
