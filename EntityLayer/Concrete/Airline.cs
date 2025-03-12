using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Airline
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AirlineID { get; set; }

        
        [Required, StringLength(100)]
        public string Airlines_Name{ get; set; }

        
        [Required, StringLength(2)]
        public string AL_IATA_Code{ get; set; }

        
        [Required, StringLength(3)]
        public string AL_ICAO_Code{ get; set; }

        
        [Required, StringLength(50)]
        public string Central_Country { get; set; }

        
        public int YearOfEstablishment { get; set; }

        
        public bool Is_It_Active { get; set; }

        
        public ICollection<Flight> Flights { get; set; }

    }
}
