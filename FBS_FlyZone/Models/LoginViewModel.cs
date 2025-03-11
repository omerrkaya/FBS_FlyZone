using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
