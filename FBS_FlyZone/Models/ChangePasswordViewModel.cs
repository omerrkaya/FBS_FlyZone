using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class ChangePasswordViewModel // Şifre değiştirme için gerekli olan modeli bu şekilde düşündüm, hatalı bulunursa değiştirilebilir.
    {
        [Required(ErrorMessage = "Mevcut şifre gereklidir")] // Mevcut şifrenin doğru girilmesi gerekmesi için "Required" kullandım.
        [Display(Name = "Mevcut Şifre")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir")] // Yeni şifrenin doğru girilmesi gerekmesi için "Required" kullandım.
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [StringLength(24, MinimumLength = 6, ErrorMessage = "Şifre en az 6, en fazla 24 karakter uzunluğunda olmalıdır")] // Şifrenin en az 6, en fazla 24 karakter uzunluğunda olması gerekmesi için "StringLength" kullandım.
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre tekrarı gereklidir")] // Yeni şifrenin tekrarının doğru girilmesi gerekmesi için "Required" kullandım.
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")] // Yeni şifrenin tekrarının doğru girilmesi gerekmesi için "Compare" kullandım. Yt videolarında vardı.
        public string? ConfirmPassword { get; set; }
    }
} 