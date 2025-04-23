using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EntityLayer.Concrete;

public class FlightSearchViewModel
{


    [Required(ErrorMessage = "Kalkış yeri zorunludur")]
    public int DepartureAirportId { get; set; }

    [Required(ErrorMessage = "Varış yeri zorunludur")]
    public int ArrivalAirportId { get; set; }

    [Required(ErrorMessage = "Gidiş tarihi zorunludur")]
    public DateTime DepartureDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; } // opsiyonel, sadece gidiş-dönüş seçildiyse gelir

    [Required(ErrorMessage = "Sınıf seçimi zorunludur"), ]
    public string? FlightClass { get; set; }

    [Range(1, 9, ErrorMessage = "Yetişkin sayısı en fazla 9 olabilir")]
    public int AdultCount { get; set; }

    [Range(0, 9, ErrorMessage = "Çocuk sayısı en fazla 9 olabilir")]
    public int ChildCount { get; set; }
}