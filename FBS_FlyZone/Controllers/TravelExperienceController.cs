using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
    public class TravelExperienceController : Controller
    {
        public IActionResult BusinessClass()
        {
            return View();
        }
        public IActionResult EconomyClass()
        {
            return View();
        }

        public IActionResult InFlightCatering()
        {

            return View();
        }

        public IActionResult InFlightEntertainment()
        {
            return View();
        }

    }
}
