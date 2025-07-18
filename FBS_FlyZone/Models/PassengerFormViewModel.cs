﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FBS_FlyZone.Models
{
    public class PassengerFormViewModel
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();


        [Required(ErrorMessage = "Uçuş seçilmedi.")]
        public int FlightId { get; set; }

        [NotMapped] // Entity Framework için
        public Flight? Flight { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi seçilmedi.")]
        public string PaymentMethod { get; set; } = string.Empty; // Initialize with empty string



    }

}
