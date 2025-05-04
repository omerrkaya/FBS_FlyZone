namespace FBS_FlyZone.Models.ViewModels
{
    public class TicketViewModel
    {
        public string PassengerName { get; set; }
        public string FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string SeatNumber { get; set; }
        public string TicketNumber { get; set; }
        public string Airline { get; set; }
    }
}

