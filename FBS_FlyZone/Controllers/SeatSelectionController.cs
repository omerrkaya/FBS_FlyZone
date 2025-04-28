using System.Security.Claims;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FBS_FlyZone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Koltukları al
            var seats = c.Seats
                          .Where(x => x.FlightID == flightId)
                          .Select(x => new SeatSelectionModel
                          {
                              SeatNumber = x.SeatNumber,
                              IsOccupied = x.IsOccupied
                          })
                          .ToList();

            // Dolu koltukları 'OccupiedSeats' listesine ekle
            var occupiedSeats = seats.Where(x => x.IsOccupied).Select(x => x.SeatNumber).ToList();

            // 'OccupiedSeats' listesini her bir SeatSelectionModel nesnesine atayın
            foreach (var seat in seats)
            {
                seat.OccupiedSeats = occupiedSeats;
            }

            // Flight ID'sini View'e gönder
            ViewBag.FlightId = flightId;
            return View(seats);
        }

        public IActionResult ConfirmSeatSelection(int flightId, string selectedSeats)
        {
            if (flightId > 0 && !string.IsNullOrWhiteSpace(selectedSeats))
            {
                var c = new Context();
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

                    var seats = selectedSeats.Split(',');

                    foreach (var seatNumber in seats)
                    {
                        // Reservation tablosundaki kayıt bulunur
                        var reservation = c.Reservations
                            .FirstOrDefault(r => r.UserID == int.Parse(userId) && r.FlightID == flightId && r.Seat_Number == "1");

                        if (reservation != null)
                        {
                            reservation.Seat_Number = seatNumber;
                            c.Reservations.Update(reservation);
                        }

                        // Seat tablosunda Seat kaydını güncelle
                        var seatEntity = c.Seats
                            .FirstOrDefault(s => s.FlightID == flightId && s.SeatNumber == seatNumber);

                        if (seatEntity != null)
                        {
                            seatEntity.IsOccupied = true;
                            c.Seats.Update(seatEntity);
                        }
                    }
                        
                    c.SaveChanges();
                }



                return RedirectToAction("ReservationSuccess"); // Başarı sayfasına yönlendir
            }
            else
            {
                return View();
            }

        }



    }
}
