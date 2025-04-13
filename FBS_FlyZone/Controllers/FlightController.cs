using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        FlightManager fm = new FlightManager(new EfFlightRepository());

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

        // Uçuş arama işlemi
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SearchedFlight(FlightSearchViewModel model)
        {
            var values = fm.GetListAll()
                .Where(f => f.DepartureAirport.AirportID == model.DepartureAirportId &&
                             f.ArrivalAirport.AirportID == model.ArrivalAirportId &&
                             f.Flight_DateTime.Date == model.DepartureDate.Date).ToList();



            if (values == null)
            {
                // Flight bulunamadığında hatayı önleyin
                return View("Error");
            }

            return View(values);
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