using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Seat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeatID { get; set; }

        [ForeignKey("Flight")]
        public int FlightID { get; set; }
        public Flight Flight { get; set; }

        [Required, StringLength(10)]
        public string SeatNumber { get; set; } // Örn: "12A", "15C"

        public bool IsOccupied { get; set; } = false;

        [StringLength(50)]
        public string? PassengerName { get; set; } // Rezerve eden yolcu adı (opsiyonel)
    }
}
