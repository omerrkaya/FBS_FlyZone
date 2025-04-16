 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Passenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PassengerID { get; set; }
        
        [Required] 
        [StringLength(100)]
        public string Passenger_Name_Surname{ get; set; }
        
       
        [Required]
        [StringLength(11)]
        public string TcNo_PasaportNo { get; set; }

        
        public DateTime Birth_Time { get; set; }

        
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        
        [Required]
        [StringLength(25)]
        public string Phone_Number { get; set; }

        
        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        public string? Gender { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

    }
}
