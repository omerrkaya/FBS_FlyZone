﻿using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Concrete;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using static System.Net.Mime.MediaTypeNames;

namespace FBS_FlyZone.Controllers
{

    public class FlightController : Controller
    {
        FlightManager fm = new FlightManager(new EfFlightRepository());
        UserManager um = new UserManager(new EfUserRepository());
        ReservationManager rm = new ReservationManager(new EfReservationRepository());
        PassengerManager pm = new PassengerManager(new EfPassengerRepository());

    

        // Ana uçuş sayfası
        [AllowAnonymous]
        public IActionResult Flight()
        {
            var values = fm.GetFlightListWithAirport();
           

            return View("~/Views/Flight/Flight.cshtml", values);
        }

        [HttpGet]
        public IActionResult SearchedFlight()
        {
            using var c = new Context();

            var values = fm.GetListAll();

            if (values == null)
            {
                // Flight bulunamadığında hatayı önleyin
                return View("Error");
            }

            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchedFlight(FlightSearchViewModel model)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            if (model.DepartureDate < DateTime.Now)
            {
                var values = fm.GetFlightListWithAirport();
                return View("Flight",values);
            }



            var userId = User.FindFirstValue("UserId");
            Context c = new Context();
            var userValues = c.Users.FirstOrDefault(x => x.UserID.ToString() == userId);

            var flights = fm.GetListAll()
                .Where(f => f.DepartureAirport.AirportID == model.DepartureAirportId &&
                            f.ArrivalAirport.AirportID == model.ArrivalAirportId &&
                            f.Flight_DateTime.Date == model.DepartureDate.Date).ToList();


            foreach (var flight in flights)
            {
                decimal childcount = model.ChildCount * 8m / 10m;
                decimal adultcount = model.AdultCount;
                flight.TotalPrice = flight.Flight_Price * Convert.ToDecimal(childcount + adultcount);

            }


            // Veriyi Session'a kaydet
            HttpContext.Session.SetInt32("AdultCount", model.AdultCount);
            HttpContext.Session.SetInt32("ChildCount", model.ChildCount);

            return View(flights);
        }

        public IActionResult FlightDetails(int id, FlightSearchViewModel modell)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            using (var c = new Context())
            {
                var userValues = c.Users.FirstOrDefault(x => x.UserID.ToString() == userId);
                if (userValues == null)
                {
                    return View("Error");
                }


                var flight = fm.GetFlightById(id);
                if (flight == null)
                {
                    return View("Error");
                }


                var adultCount = HttpContext.Session.GetInt32("AdultCount") ?? 0;

                var childCount = HttpContext.Session.GetInt32("ChildCount") ?? 0;


                int totalPassengerCount = adultCount + childCount;

                // Passenger listesi oluştur
                var passengers = new List<Passenger>();
                for (int i = 0; i < totalPassengerCount; i++)
                {
                    passengers.Add(new Passenger());
                }

                var model = new PassengerFormViewModel
                {
                    AdultCount = adultCount,
                    ChildCount = childCount,
                    Passengers = passengers,
                    FlightId = id,
                    Flight = flight
                };

                return View(model); // Razor View'un adı "PassengerForm.cshtml" olacak
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation(PassengerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  // Ya da ViewBag’e bas
                }
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı yoksa hata döndür

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login");  // Eğer kullanıcı giriş yapmamışsa login sayfasına yönlendir
                }


                // Her bir yolcu için ayrı ayrı işlem yapılacak
                foreach (var p in model.Passengers)
                {
                    Passenger passenger;

                    // Kullanıcının daha önce aynı bilgilerle bir Passenger kaydı var mı?
                    var existingPassenger = pm.GetPassengerByUserIdAndTcNo(p.TcNo_PasaportNo);

                    if (existingPassenger != null)
                    {
                        passenger = existingPassenger;
                    }
                    else
                    {
                        passenger = new Passenger
                        {
                            Passenger_Name_Surname = p.Passenger_Name_Surname,
                            TcNo_PasaportNo = p.TcNo_PasaportNo,
                            Birth_Time = p.Birth_Time,
                            Gender = p.Gender,
                            Email = p.Email,
                            Phone_Number = p.Phone_Number,
                            Nationality = p.Nationality,
                            UserID = int.Parse(userId)
                        };

                        pm.AddPassenger(passenger);
                    }

                    var existingReservation = rm.GetReservationByPassengerAndFlight(passenger.PassengerID, model.FlightId);
                    if (existingReservation != null)
                    {
                        Console.WriteLine("Bu yolcu için zaten bir rezervasyon var.");
                    }

                    var reservation = new Reservation
                    {
                        FlightID = model.FlightId,
                        PassengerID = passenger.PassengerID,
                        Reservation_Date = DateTime.Now,
                        Reservation_Status = "Onay Bekliyor",
                        Payment_Status = "Ödenmedi",
                        Payment_Method = model.PaymentMethod,
                        Seat_Number = "1", // Bu kısmı ileride dinamik koltuk seçimiyle güncelleyebilirsin
                        UserID = int.Parse(userId)
                    };

                    rm.AddReservation(reservation);
                }
                return RedirectToAction("SelectSeat", "SeatSelection", new { flightId = model.FlightId }); // Rezervasyon işlemi tamamlandıktan sonra koltuk seçimi sayfasına yönlendir
            }

            return RedirectToAction("FlightDetails", new {  id = model.FlightId });  // Eğer model geçerli değilse tekrar formu göster
        }


        // Arama işlemi
        public IActionResult Search(string keyword)
        {
            ViewBag.SearchKeyword = keyword;
            return View();
        }


    }
}