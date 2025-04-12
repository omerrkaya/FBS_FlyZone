using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Airport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AirportID { get; set; }

       
        [Required, StringLength(50)]
        public string Airport_Name { get; set; }

        
        [Required,StringLength(3)]
        public string IATA_Code { get; set; }

        
        [Required, StringLength(4)]
        public string ICAO_Code { get; set; }

        
        [Required, StringLength(50)]
        public string AP_City { get; set; }

        
        [Required, StringLength(75)]
        public string AP_Country { get; set; }

       
    }
}
