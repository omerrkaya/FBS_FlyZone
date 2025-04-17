using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Concrete;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

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

            return View(values);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchedFlight()
        {

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

            var userId = User.FindFirstValue("UserId");
            Context c = new Context();
            var userValues = c.Users.FirstOrDefault(x => x.UserID.ToString() == userId);

            var flights = fm.GetListAll()
                .Where(f => f.DepartureAirport.AirportID == model.DepartureAirportId &&
                            f.ArrivalAirport.AirportID == model.ArrivalAirportId &&
                            f.Flight_DateTime.Date == model.DepartureDate.Date).ToList();


            foreach (var flight in flights)
            {
                var TotalFlights = flight.Flight_Price * (model.AdultCount + model.ChildCount);
            }

            return View(flights);
        }


        public IActionResult FlightDetails(int id)
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Kullanıcının ID'sini almak için
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");  // Eğer kullanıcı girmemişse login sayfasına yönlendir
            }

            using (var c = new Context())
            {
                var userValues = c.Users.FirstOrDefault(x => x.UserID.ToString() == userId);
                if (userValues == null)
                {
                    return View("Error");  // Kullanıcı veritabanında bulunamadıysa hata sayfası
                }

                var availableSeats = c.Seats
                        .Where(s => s.FlightID == id && !s.IsOccupied)
                        .ToList();
                var flight = fm.GetFlightById(id);  // Uçuş bilgilerini al
                if (flight == null)
                {
                    return View("Error");  // Uçuş bulunamazsa hata sayfası
                }

                var reservationViewModel = new ReservationViewModel
                {
                    SeatNumber="1",
                    Flight = flight,
                    User = userValues,
                    Passenger = new Passenger()  // Yeni Passenger nesnesi oluştur
                };

                return View(reservationViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation(ReservationViewModel model)
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


                // Kullanıcının daha önce kaydedilmiş bir Passenger'ı var mı diye kontrol et
                var existingPassenger = pm.GetPassengerByUserId(int.Parse(userId));

                Passenger passenger;

                if (existingPassenger != null)
                {
                    // Eğer yolcu zaten varsa, mevcut olanı kullan
                    passenger = existingPassenger;
                }
                else
                {
                    // Yeni Passenger oluştur
                    passenger = new Passenger
                    {
                        Passenger_Name_Surname = model.Passenger.Passenger_Name_Surname,
                        TcNo_PasaportNo = model.Passenger.TcNo_PasaportNo,
                        Birth_Time = model.Passenger.Birth_Time,
                        Gender = model.Passenger.Gender,
                        Email = model.Passenger.Email,
                        Phone_Number = model.Passenger.Phone_Number,
                        Nationality = model.Passenger.Nationality,
                        UserID = int.Parse(userId),  // Kullanıcının ID'sini al
                    };
                    pm.AddPassenger(passenger);  // Yeni yolcuyu veritabanına ekle
                }



                //// Yeni Reservation oluştur
                //var reservation = new Reservation
                //{
                //    FlightID = model.Flight.FlightID,
                //    PassengerID = passenger.PassengerID,
                //    Reservation_Date = DateTime.Now,
                //    Reservation_Status = "Onay Bekliyor",
                //    Payment_Method = model.PaymentMethod,
                //    Seat_Number = "1",
                //    UserID = int.Parse(userId)
                //};

                //rm.AddReservation(reservation); // Yeni rezervasyonu veritabanına ekle

                return RedirectToAction("Index", "Payment"); // Başarıyla rezervasyon tamamlandı
            }

            return View(model);  // Eğer model geçerli değilse tekrar formu göster
        }


        // Arama işlemi
        public IActionResult Search(string keyword)
        {
            ViewBag.SearchKeyword = keyword;
            return View();
        }

    }
}