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
        [ForeignKey("Departure_Airport")]
        public int Departure_Airport { get; set; }
        public Airport DepartureAirport { get; set; }  

        
        // Foreign Key: Arrival Airport
        [ForeignKey("Arrival_Airport")]
        public int Arrival_Airport { get; set; }
        public Airport ArrivalAirport { get; set; }  

        
        [StringLength(10)]
        public string Flight_Code { get; set; }

        
        public DateTime Flight_DateTime { get; set; }

        
        [Column(TypeName = "time")]
        public TimeSpan Estimated_Time { get; set; }

        
        // Foreign Key: Aircraft
        [ForeignKey("Aircraft")]
        public int AircraftID { get; set; }
        public Aircraft Aircraft { get; set; }


        public decimal Flight_Price { get; set; }


    }
}
