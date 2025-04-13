using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Concrete;
using System.Security.Claims;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        FlightManager fm = new FlightManager(new EfFlightRepository());
        UserManager um = new UserManager(new EfUserRepository());

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
            return View();
        }
       
        // Arama işlemi
        public IActionResult Search(string keyword)
        {
            ViewBag.SearchKeyword = keyword;
            return View();
        }
        
    }
}