using System.Security.Claims;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FBS_FlyZone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using Newtonsoft.Json;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
    public class SeatSelectionController : Controller
    {
        ReservationManager rm = new ReservationManager(new EfReservationRepository());
        SeatManager sm = new SeatManager(new EfSeatRepository());

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult SelectSeat(int flightId)
        {
            var c = new Context();

            // Kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");



            // Kullanıcının bu uçuş için rezervasyonlarını bul
            var userReservations = c.Reservations
                .Where(r => r.UserID == int.Parse(userId) && r.FlightID == flightId)
                .ToList();


            // En son rezervasyon TARİHİNİ bul (sadece gün/ay/yıl)
            var latestReservationDate = c.Reservations
                .Where(r => r.UserID == int.Parse(userId) && r.FlightID == flightId)
                .Max(r => r.Reservation_Date).Date; // Sadece tarih kısmını al, saat bilgisini at


            // Bu TARİHTE yapılan tüm rezervasyonları getir (saat önemsiz)
            var reservationsWithPassengers = c.Reservations
                .Where(r => r.UserID == int.Parse(userId) &&
                           r.FlightID == flightId &&
                           r.Reservation_Date.Date == latestReservationDate)
                .Include(r => r.Passenger)
                .ToList();



            // Uçuş için dolu koltukları bul
            var occupiedSeats = c.Seats
                .Where(s => s.FlightID == flightId && s.IsOccupied)
                .Select(s => s.SeatNumber)
                .ToList();


            // View modeli oluştur
            var viewModel = new List<SeatSelectionModel>();

            // Her rezervasyon için model ekle
            foreach (var reservation in reservationsWithPassengers)
            {

                viewModel.Add(new SeatSelectionModel
                {
                    ReservationId = reservation.ReservationID,
                    FlightId = flightId,
                    OccupiedSeats = occupiedSeats,
                    PassengerName = reservation.Passenger.Passenger_Name_Surname,
                    PassengerId = reservation.PassengerID,
                    SeatNumber = reservation.Seat_Number
                });
            }


            return View(viewModel);
        }



        public IActionResult ConfirmSeatSelection(int flightId, string passengerSeats)
        {
            if (flightId > 0 && !string.IsNullOrWhiteSpace(passengerSeats))
            {
                var c = new Context();
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

                    // JSON verisini parse et
                    var passengerSeatData = JsonConvert.DeserializeObject<List<PassengerSeatData>>(passengerSeats);

                    // Transaction başlat
                    using (var transaction = c.Database.BeginTransaction())
                    {
                        try
                        {
                            // Her bir yolcu ve koltuk için
                            foreach (var data in passengerSeatData)
                            {
                                if (string.IsNullOrEmpty(data.SeatNumber))
                                    continue;

                                // Koltuğun durumunu kontrol et
                                Seat? seatEntity = c.Seats.FirstOrDefault(s => s.FlightID == flightId && s.SeatNumber == data.SeatNumber);

                                if (seatEntity == null)
                                {
                                    // Koltuk veritabanında yoksa ekle
                                    seatEntity = new Seat
                                    {
                                        FlightID = flightId,
                                        SeatNumber = data.SeatNumber,
                                        IsOccupied = true
                                    };
                                    c.Seats.Add(seatEntity);
                                }
                                else if (seatEntity.IsOccupied)
                                {
                                    // Koltuk dolu
                                    transaction.Rollback();
                                    return RedirectToAction("SelectSeat", new { flightId, error = "SeatAlreadyTaken" });
                                }
                                else
                                {
                                    // Koltuğu dolu olarak işaretle
                                    seatEntity.IsOccupied = true;
                                    c.Seats.Update(seatEntity);
                                }

                                // Rezervasyonu bul ve güncelle
                                var reservation = c.Reservations.Find(int.Parse(data.ReservationId));
                                if (reservation != null)
                                {
                                    reservation.Seat_Number = data.SeatNumber;
                                    c.Reservations.Update(reservation);
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return RedirectToAction("Error", new { message = "Rezervasyon bulunamadı!" });
                                }
                            }

                            c.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index", "Payment", new { flighttId = flightId });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return RedirectToAction("Error", new { message = ex.Message });
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("SelectSeat");
            }

        }

        // Yardımcı sınıf
        public class PassengerSeatData
        {
            public string PassengerId { get; set; }
            public string ReservationId { get; set; }
            public string SeatNumber { get; set; }
        }




    }
}
