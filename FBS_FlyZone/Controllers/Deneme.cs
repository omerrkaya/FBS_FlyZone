using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;


namespace FBS_FlyZone.Controllers
{
    public class Deneme : Controller
    {
        FlightManager fm = new FlightManager(new DataAccessLayer.EntityFramework.EfFlightRepository());
        public IActionResult Index()
        {
            var values = fm.GetListAll();

            return View(values);
        }
    }
}
