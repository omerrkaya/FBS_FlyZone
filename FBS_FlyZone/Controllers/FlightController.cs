using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        // Ana uçuş sayfası
        [AllowAnonymous]
        public IActionResult Flight()
        {
            return View();
        }

        // Uçuş arama işlemi
        [HttpPost]
        public IActionResult SearchFlight(FlightSearchViewModel model)
        {
            // Uçuş arama işlemlerini burada yapmamız gerek diye düşündüm.
            return View();
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