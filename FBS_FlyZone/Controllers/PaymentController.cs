//PaymentController.cs ödeme sayfası controller ı.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBS_FlyZone.Models;
using System;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller 
    {

        
        [HttpGet] //get işlemi yapılır.
        public IActionResult Index()
        {

            return View(new PaymentViewModel()); //ödeme sayfasına yönlendirilir.
        }
        
        [HttpPost] //post işlemi yapılır.
        public IActionResult Index(PaymentViewModel model,  decimal totalPrice, int adultCount, int childCount)
        {
            if (!ModelState.IsValid)
            {
                return View(model); //model hatalı ise view içine geri döner. Modele yanlış veri döndüğünde çalışır.
                //hata mesajlarının görüntülenmesi için.
            }
            
            // Gerçek uygulamada ödeme işlemleri burada yapılır
            
            // Başarılı ödeme simülasyonu, gerçek veri olmasa da ödeme işlemini simüle eder. k
            TempData["SuccessMessage"] = "Ödemeniz başarıyla alındı. Biletiniz mail adresinize gönderildi.";
            
            return RedirectToAction("PaymentSuccess"); //başarılı ödeme sayfasına yönlendirilir. Gerekirse burası değiştirilebilir(başarılı ödeme sayfası).
        }
        
        public IActionResult PaymentSuccess() //başarılı ödeme sayfasına yönlendirilir. Gerekirse burası değiştirilebilir.
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult PaymentFailed() //başarısız ödeme sayfasına yönlendirilir. Gerekirse burası değiştirilebilir.
        {
            return View();
        }
    }
} 