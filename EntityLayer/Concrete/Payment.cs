using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }


        [ForeignKey("Reservation")]
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }


        public DateTime Payment_Date { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        public decimal Payment_Amount { get; set; }


        [Required, StringLength(50)]
        public string Payment_Method { get; set; }


        [Required, StringLength(20)]
        public string Payment_Status { get; set; }
    }
}
