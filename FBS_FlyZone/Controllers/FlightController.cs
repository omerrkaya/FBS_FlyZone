using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        public IActionResult Flight()
        {
            return View();
        }
        public IActionResult FlightDetails()
        {
            return View();
        }
        public IActionResult Girisyap()
        {
            return View();
        }
         public IActionResult Kayitol()
        {
            return View();
        }
       
        public IActionResult BusinessClass()
        {
            return View();
        }
        public IActionResult UcusDurumu()
        {
            return View();
        }
        public IActionResult CheckinBiletyonetimi()
        {
            return View();
        }

    }
}
