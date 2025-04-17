using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Kart numarası zorunludur")]
        //[CreditCard(ErrorMessage = "Geçerli bir kart numarası giriniz")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "Son kullanma tarihi zorunludur")]
        public string? ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV zorunludur")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalıdır")]
        public string? CVV { get; set; }

        [Required(ErrorMessage = "Kart sahibi adı zorunludur")]
        public string? CardHolderName { get; set; }
    }
}