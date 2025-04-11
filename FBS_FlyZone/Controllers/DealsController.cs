using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
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
