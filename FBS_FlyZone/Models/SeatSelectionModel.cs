using EntityLayer.Concrete;

namespace FBS_FlyZone.Models
{
    public class SeatSelectionModel
    {
        public int ReservationId { get; set; }  // Hangi rezervasyon için koltuk seçiliyor
        public List<string> SelectedSeats { get; set; } = new List<string>();

        public int FlightId { get; set; } // Uçuş ID'si
        public string SeatNumber { get; set; }
        public bool IsOccupied { get; set; } // Koltuk dolu mu boş mu
        public List<string> OccupiedSeats { get; set; } = new List<string>(); // Dolu koltuklar
    }

}
