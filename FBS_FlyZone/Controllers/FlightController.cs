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

        public IActionResult BusinessClass()
        {
            return View();
        }
        public IActionResult EconomyClass()
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
        public IActionResult seatselection()
        {
            return View();
        }

        public IActionResult ExtraBaggage()
        {
            return View();
        }
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
        
        public IActionResult Fleet()
        {
          
            return View();
        }
        public IActionResult InFlightCatering()
        {

            return View();
        }
        public IActionResult GuideVideos()
        {

            return View();
        }
        public IActionResult InflightEntertainment()
        {

            return View();
        }
        public IActionResult TransportationInformation()
        {

            return View();
        }
        public IActionResult BoardingPass()
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