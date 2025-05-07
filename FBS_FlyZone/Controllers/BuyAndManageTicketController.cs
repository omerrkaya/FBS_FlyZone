using System.Security.Claims;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FBS_FlyZone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FBS_FlyZone.Controllers
{

    
 
    public class BuyAndManageTicketController : Controller
    {
        Context context = new Context();
        public IActionResult FlightStatus()
        {
            return View();
        }
        public IActionResult CheckInTicketManagement()
        {
            return View();
        }

        public IActionResult SeatSelection()
        {
            return View();
        }
        public IActionResult ExtraBaggage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sorgulama(string TC_No, string flightCode)
         {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim.Value);

            var reservations = context.Reservations
                .Include(r => r.Flight)
                    .ThenInclude(f => f.DepartureAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.ArrivalAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Airline)
                .Include(r => r.Passenger)
                .Where(r => r.UserID == userId && r.Passenger.TcNo_PasaportNo == TC_No && r.Flight.Flight_Code == flightCode);

            List<Reservation> filteredReservations = new List<Reservation>();

            foreach (var item in reservations)
            {
                filteredReservations.Add(item);
            }

            return View("Sorgulama",filteredReservations);

        }
    }
}
