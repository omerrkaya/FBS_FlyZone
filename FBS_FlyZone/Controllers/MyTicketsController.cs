using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Concrete; // Context buradan geliyor
using FBS_FlyZone.Models.ViewModels; // TicketViewModel için
using System.Security.Claims;
using EntityLayer.Concrete;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FBS_FlyZone.Controllers
{

    public class MyTicketsController : Controller
    {
       


        public ActionResult MyTickets()
        {
            using var _context = new Context();
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim.Value);

            var reservations = _context.Reservations
                .Include(r => r.Flight)
                    .ThenInclude(f => f.DepartureAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.ArrivalAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Airline)
                .Include(r => r.Passenger)
                .Where(r => r.UserID == userId)
                .Select(r => new TicketViewModel
                {
                    PassengerName = r.Passenger.Passenger_Name_Surname,
                    FlightNumber = r.Flight.Flight_Code,
                    Departure = r.Flight.DepartureAirport.Airport_Name,
                    Arrival = r.Flight.ArrivalAirport.Airport_Name,
                    DepartureTime = r.Flight.Flight_DateTime,
                    ArrivalTime = r.Flight.Flight_DateTime.Add(r.Flight.Estimated_Time),
                    SeatNumber = r.Seat_Number,
                    TicketNumber = r.ReservationID.ToString(),
                    Airline = r.Flight.Airline.Airlines_Name
                })
                .ToList();


            return View(reservations);
        }


    }
}

