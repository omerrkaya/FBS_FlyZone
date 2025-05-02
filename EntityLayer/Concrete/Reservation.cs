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

        public string Payment_Method { get; set; }
        public string Payment_Status { get; set; }




        [ForeignKey("User")]
        [Required]  // Bu, UserID'nin boş olamayacağını belirtir.
        public int UserID { get; set; }
        public User User { get; set; }


        [ForeignKey("Payment")]
        public int? PaymentID { get; set; }  // Nullable olarak tanımlandı
        public Payment Payment { get; set; } // Navigation property


        public ICollection<Flight> Flights { get; set; }
    }
}
