using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FBS_FlyZone.Models
{
    public class PassengerFormViewModel
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public List<Passenger> Passengers { get; set; }


        [Required(ErrorMessage = "Uçuş seçilmedi.")]
        public int FlightId { get; set; }

        [NotMapped] // Entity Framework için
        public Flight? Flight { get; set; }

        public string PaymentMethod { get; set; } // Ödeme yöntemi (Kredi Kartı, Havale, Kapıda Ödeme vb.)

    }
}
