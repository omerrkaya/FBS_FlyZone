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
        public string Passenger_Name_Surname { get; set; }

        [Required(ErrorMessage = "TC No 11 karakter olmalı ve sadece sayı girilmeli")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC No 11 haneli sayı olmalıdır.")]
        [StringLength(11)]
        public string TcNo_PasaportNo { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Passenger),nameof(ValidateBirthDate))]
        public DateTime Birth_Time { get; set; }


        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [StringLength(50)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [StringLength(25)]
        public string Phone_Number { get; set; }


        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        public string? Gender { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }


        public static ValidationResult ValidateBirthDate(DateTime birthDate, ValidationContext context)
        {
            if (birthDate > DateTime.Now)
            {
                return new ValidationResult("Doğum tarihi gelecekte olamaz.", new[] { context.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}