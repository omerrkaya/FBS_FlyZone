using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
    public class DealsController : Controller
    {
        FlightManager fm = new FlightManager(new EfFlightRepository());

        public IActionResult Flightdeals()
        {
            var values = fm.GetFlightListWithAirport();  

            return View("~/Views/Deals/Flightdeals.cshtml", values);
        }
        public IActionResult StudentDiscount()
        {
            return View();
        }
        public IActionResult Offer()
        {
            return View();
        }
    }
}
