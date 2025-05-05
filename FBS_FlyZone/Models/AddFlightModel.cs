using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Concrete;

namespace FBS_FlyZone.Models
{
    public class AddFlightModel
    {
        public List<Flight> Flights { get; set; }

        public Flight Flight { get; set; }

        public string Flight_Code { get; set; }

        [Required(ErrorMessage ="Lütfen Doğru Tarih Seçiniz")]
        public DateTime Flight_DateTime { get; set; }

        public string Departure_Airport { get; set; }
        public string Arrival_Airport { get; set; }
        public string Aircraft { get; set; }
        public decimal Flight_Price { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan Estimated_Time { get; set; }

    }
}
