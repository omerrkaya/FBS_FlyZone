using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FBS_FlyZone.Controllers
{

    public class Deneme : Controller
    {
        FlightManager fm = new FlightManager(new EfFlightRepository());
        [AllowAnonymous]
        public IActionResult Index()
        {
            var values = fm.GetFlightListWithAirport();

            return View(values);
        }
    }
}
