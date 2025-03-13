using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;

public class BookingController : Controller
{
    public IActionResult MyBookings()
    {
        return View();
    }

    public IActionResult BookingDetails(int id)
    {
        return View();
    }


    [HttpPost]
    public IActionResult ConfirmPayment(PaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Ödeme işlemlerini burada yapıyoruz. sıkıntı çıkaersa düzeltirilir.
        }
        return View(model);
    }

    public IActionResult CancelBooking(int id)
    {
        return View();
    }

    public IActionResult Payment(PaymentViewModel model)
    {
        return View(model);
    }

}