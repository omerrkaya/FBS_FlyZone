using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Aircraft
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AircraftID { get; set; }
        
        [ForeignKey("Aircraft_Firm")]
        public int Aircraft_Firm { get; set; }
        
        [Required, StringLength(50)]
        public string Aircraft_Model { get; set; }
        
        public int Number_Of_Seats { get; set; }
        
        public int Range_KM { get; set; }
        
        public int Year_Of_Producation { get; set; }

        public ICollection<Flight> Flights { get; set; }
    }
}
