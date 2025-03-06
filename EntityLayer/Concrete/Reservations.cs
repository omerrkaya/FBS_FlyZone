using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Reservations
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        [ForeignKey("FlightID")]
        public int FlightID { get; set; }

        [ForeignKey("PassengerID")]
        public int PassengerID { get; set; }

        [Required, StringLength(10)]
        public string Seat_Number { get; set; }
       
        
        public DateTime Reservation_Date { get; set; }
        
        
        public string Reservation_Status { get; set; }
    }
}
