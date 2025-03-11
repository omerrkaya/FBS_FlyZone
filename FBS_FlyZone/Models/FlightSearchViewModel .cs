using System;
using System.ComponentModel.DataAnnotations;

public class FlightSearchViewModel
{
    [Required(ErrorMessage = "Kalkış yeri zorunludur")]
    public string? FromLocation { get; set; }

    [Required(ErrorMessage = "Varış yeri zorunludur")] 
    public string? ToLocation { get; set; }

    [Required(ErrorMessage = "Gidiş tarihi zorunludur")]
    public DateTime DepartureDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; }

    [Range(1, 9, ErrorMessage = "Yetişkin sayısı 1-9 arasında olmalıdır")]
    public int AdultCount { get; set; } = 1;

    [Range(0, 9, ErrorMessage = "Çocuk sayısı 0-9 arasında olmalıdır")]
    public int ChildCount { get; set; } = 0;
}