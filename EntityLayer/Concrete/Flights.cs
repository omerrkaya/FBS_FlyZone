using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Flights
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightID { get; set; }

        [ForeignKey("AirlineID")]
        public int AirlineID { get; set; }
        
        [ForeignKey("Departure_Airport")]
        public int Departure_Airport { get; set; }
        
        [ForeignKey("Arrival_Airport")]
        public int Arrival_Airport { get; set; }
        
        [StringLength(10)]
        public string Flight_Code { get; set; }
        
        public DateTime Flight_DateTime { get; set; }
        
        [Column(TypeName = "time")]
        public TimeSpan Estimated_Time { get; set; }

        [ForeignKey("AircraftID")]
        public int AircraftID { get; set; }

        public decimal Flight_Price { get; set; }
    }
}
