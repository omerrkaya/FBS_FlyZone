using EntityLayer.Concrete;
using System.ComponentModel.DataAnnotations;

namespace FBS_FlyZone.Models
{
    public class SeatSelectionModel
    {
        public int ReservationId { get; set; }  // Hangi rezervasyon için koltuk seçiliyor
        public List<string> SelectedSeats { get; set; } = new List<string>();

        public int FlightId { get; set; } // Uçuş ID'si
        [Required]
        public string SeatNumber { get; set; } = string.Empty; // Initialize with empty string to satisfy non-nullable requirement
        public bool IsOccupied { get; set; } // Koltuk dolu mu boş mu
        public List<string> OccupiedSeats { get; set; } = new List<string>(); // Dolu koltuklar
    }
}
