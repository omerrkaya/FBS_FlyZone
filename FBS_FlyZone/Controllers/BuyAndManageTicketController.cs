using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
 
    public class BuyAndManageTicketController : Controller
    {
        public IActionResult FlightStatus()
        {
            return View();
        }
        public IActionResult CheckInTicketManagement()
        {
            return View();
        }

        public IActionResult SeatSelection()
        {
            return View();
        }
        public IActionResult ExtraBaggage()
        {
            return View();
        }

    }
}
