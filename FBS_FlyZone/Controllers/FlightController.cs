using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        public IActionResult Flight()
        {
            return View();
        }
    }
}
