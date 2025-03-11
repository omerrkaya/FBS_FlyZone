using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "User Name&Surname is required")]
        [StringLength(51)]
        public string User_Name_Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(24)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "User Role is required")]
        [StringLength(10)]
        public string UserRole { get; set; }
        public DateTime RegisterationDate { get; set; }
    }
}
