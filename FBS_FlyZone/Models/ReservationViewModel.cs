using EntityLayer.Concrete;

namespace FBS_FlyZone.Models
{
    public class ReservationViewModel
    {
        public Flight Flight { get; set; }
        public User? User { get; set; }
        public Passenger? Passenger { get; set; }

        // Ek alanlar
        public string PaymentMethod { get; set; }
        public string? SeatNumber { get; set; }  // Eğer koltuk seçimi varsa

        public List<Seat> Availableseats { get; set; }
    }

}
