using EntityLayer.Concrete;

namespace FBS_FlyZone.Models
{
    public class SeatSelectionModel
    {
        public int ReservationId { get; set; }
        public List<string> SelectedSeats { get; set; } = new List<string>();
        public int FlightId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsOccupied { get; set; }
        public List<string> OccupiedSeats { get; set; } = new List<string>();

        public string PassengerName { get; set; }
        public int PassengerId { get; set; }
    }


}
