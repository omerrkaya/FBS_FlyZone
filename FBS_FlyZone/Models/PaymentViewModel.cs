using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class PaymentViewModel
    {
        // Kart bilgileri
        [Display(Name = "Kart Üzerindeki Ad ve Soyad")]
        [Required(ErrorMessage = "Kart üzerindeki ad ve soyad zorunludur")]
        public string CardHolderName { get; set; }

        [Display(Name = "Kart Numarası")]
        [Required(ErrorMessage = "Kart numarası zorunludur")]
        [RegularExpression(@"^(\d{4}\s?){3}\d{4}$", ErrorMessage = "Geçerli bir kart numarası giriniz")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma Tarihi")]
        [Required(ErrorMessage = "Son kullanma tarihi zorunludur")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Geçerli bir son kullanma tarihi giriniz (AA/YY)")]
        public string ExpiryDate { get; set; }

        [Display(Name = "CVV")]
        [Required(ErrorMessage = "CVV zorunludur")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Geçerli bir CVV giriniz")]
        public string CVV { get; set; }

        // Fatura bilgileri
        [Display(Name = "E-posta Adresi")]
        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Display(Name = "Şehir")]
        [Required(ErrorMessage = "Şehir zorunludur")]
        public string City { get; set; }

        [Display(Name = "Posta Kodu")]
        [Required(ErrorMessage = "Posta kodu zorunludur")]
        public string PostalCode { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Adres zorunludur")]
        public string Address { get; set; }

        // Rezervasyon ve ödeme bilgileri
        public List<Reservation> Reservations { get; set; }
        public Flight Flight { get; set; }
        public decimal TotalPrice { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }

        public PaymentViewModel()
        {
            Reservations = new List<Reservation>();
        }
    }
}
