using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Flight
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightID { get; set; }

        // Foreign Key: Airline
        [ForeignKey("Airline")]
        public int AirlineID { get; set; }
        public Airline Airline { get; set; }

        // Foreign Key: Departure Airport
        [ForeignKey("DepartureAirport")]
        public int DepartureAirportID { get; set; }
        public Airport DepartureAirport { get; set; }

        // Foreign Key: Arrival Airport
        [ForeignKey("ArrivalAirport")]
        public int ArrivalAirportID { get; set; }
        public Airport ArrivalAirport { get; set; }

        [StringLength(10)]
        public string Flight_Code { get; set; }

        // Added missing properties that are referenced in views and controllers
        [StringLength(10)]
        public string FlightNumber { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(10)]
        public string Gate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime Flight_DateTime { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan Estimated_Time { get; set; }

        // Foreign Key: Aircraft
        [ForeignKey("Aircraft")]
        public int AircraftID { get; set; }
        public Aircraft Aircraft { get; set; }

        public decimal Flight_Price { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}