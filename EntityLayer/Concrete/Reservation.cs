using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Reservation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        
        [ForeignKey("Flight")]
        public int FlightID { get; set; }
        public Flight Flight { get; set; }

        
        [ForeignKey("Passenger")]
        public int PassengerID { get; set; }
        public Passenger Passenger { get; set; }

        
        [Required, StringLength(10)]
        public string Seat_Number { get; set; }
       
        
        public DateTime Reservation_Date { get; set; }
        
        
        public string Reservation_Status { get; set; }


        public ICollection<Flight> Flights { get; set; }
    }
}
