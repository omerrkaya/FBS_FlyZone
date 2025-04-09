using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    public class DealsController : Controller
    {
        public IActionResult Flightdeals()
        {
            return View();
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
