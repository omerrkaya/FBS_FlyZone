using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;

namespace FBS_FlyZone.Controllers
{
    public class FlightController : Controller
    {
        // Ana uçuş sayfası
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
        // Giriş sayfası burada yapılıyor.
        public IActionResult Girisyap()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Girisyap(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Giriş işlemleri
            }
            return View(model);
        }

        // Kayıt sayfası burada yapılıyor.
        public IActionResult Kayitol()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Kayitol(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kayıt işlemleri
            }
            return View(model);
        }
    }
}