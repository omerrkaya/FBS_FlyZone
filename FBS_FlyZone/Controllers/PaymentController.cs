using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBS_FlyZone.Models;
using System;
using System.Linq;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EntityLayer.Concrete;

namespace FBS_FlyZone.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            using (var c = new Context())
            {
                // Son rezervasyon tarihini bul
                var latestReservationDate = c.Reservations
                    .Where(r => r.UserID == int.Parse(userId) && r.Reservation_Status == "Onay Bekliyor")
                    .Max(r => (DateTime?)r.Reservation_Date);

                if (latestReservationDate == null)
                {
                    // Rezervasyon bulunamadı
                    return RedirectToAction("Index", "Home");
                }

                // Son rezervasyonları getir
                var reservations = c.Reservations
                    .Where(r => r.UserID == int.Parse(userId) &&
                               r.Reservation_Date.Date == latestReservationDate.Value.Date &&
                               r.Reservation_Status == "Onay Bekliyor")
                    .Include(r => r.Passenger)
                    .Include(r => r.Flight)
                    .ThenInclude(f => f.Airline)
                    .Include(r => r.Flight)
                    .ThenInclude(f => f.DepartureAirport)
                    .Include(r => r.Flight)
                    .ThenInclude(f => f.ArrivalAirport)
                    .ToList();

                if (!reservations.Any())
                {
                    // Rezervasyon bulunamadı
                    return RedirectToAction("Index", "Home");
                }

                // İlk uçuşu al (tüm rezervasyonlar aynı uçuş için olmalı)
                var flight = reservations.First().Flight;

                // Diğer controller'da oku
                var adultCount = HttpContext.Session.GetInt32("AdultCount") ?? 0;
                var childCount = HttpContext.Session.GetInt32("ChildCount") ?? 0;

                // Toplam fiyatı hesapla
                decimal totalPrice = flight.Flight_Price * Convert.ToDecimal((childCount * 0.8m) + adultCount);

                // View model oluştur
                var model = new PaymentViewModel
                {
                    Reservations = reservations,
                    Flight = flight,
                    FlightID = flight.FlightID,
                    ReservationIDs = reservations.Select(r => r.ReservationID).ToList(),
                    TotalPrice = totalPrice,
                    AdultCount = adultCount,
                    ChildCount = childCount
                };

                // Kullanıcı bilgilerini otomatik doldur
                var user = c.Users.FirstOrDefault(u => u.UserID == int.Parse(userId));
                if (user != null)
                {
                    model.Email = user.Email;
                    // Diğer kullanıcı bilgileri varsa ekleyin
                }

                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PaymentViewModel model)
        {
            using (var c = new Context())
            {
                try
                {
                    // Debug için model kontrolü
                    Console.WriteLine("POST işlemi başladı");

                    if (model == null)
                    {
                        Console.WriteLine("HATA: Tüm model null");
                        ModelState.AddModelError("", "Model null olarak geldi.");
                        return View(new PaymentViewModel()); // Boş model döndür
                    }

                    Console.WriteLine($"FlightID: {model.FlightID}");
                    Console.WriteLine($"ReservationIDs Count: {model.ReservationIDs?.Count ?? 0}");

                    // Flight nesnesini yükle
                    var flight = c.Flights
                        .Include(f => f.DepartureAirport)
                        .Include(f => f.ArrivalAirport)
                        .Include(f => f.Airline)
                        .Include(f => f.Aircraft)
                        .FirstOrDefault(f => f.FlightID == model.FlightID);

                    if (flight == null)
                    {
                        Console.WriteLine($"HATA: Flight nesnesi bulunamadı. FlightID: {model.FlightID}");
                        ModelState.AddModelError("", "Uçuş bilgisi bulunamadı.");
                        return View(model);
                    }

                    // Rezervasyonları yükle
                    var reservations = new List<Reservation>();
                    if (model.ReservationIDs != null && model.ReservationIDs.Any())
                    {
                        reservations = c.Reservations
                            .Where(r => model.ReservationIDs.Contains(r.ReservationID))
                            .Include(r => r.Passenger)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.Airline)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.DepartureAirport)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.ArrivalAirport)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.Aircraft)
                            .ToList();

                        if (!reservations.Any())
                        {
                            Console.WriteLine("HATA: Hiç rezervasyon bulunamadı");
                            ModelState.AddModelError("", "Rezervasyon bilgileri bulunamadı.");
                            return View(model);
                        }
                    }
                    else
                    {
                        Console.WriteLine("HATA: ReservationIDs null veya boş");
                        ModelState.AddModelError("", "Rezervasyon ID'leri bulunamadı.");
                        return View(model);
                    }

                    // Model'i doldur
                    model.Flight = flight;
                    model.Reservations = reservations;

                    // ÖNEMLİ: ModelState'i temizle
                    ModelState.Clear();

                    // Modeli manuel olarak doğrula
                    if (!TryValidateModel(model))
                    {
                        Console.WriteLine("HATA: Model doğrulama başarısız");
                        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                        {
                            Console.WriteLine($"- Hata: {error.ErrorMessage}");
                        }
                        return View(model);
                    }

                    Console.WriteLine("Model başarıyla doğrulandı, işleme devam ediliyor...");

                    // Ödeme işlemi
                    var payment = new Payment
                    {
                        ReservationID = reservations.FirstOrDefault()?.ReservationID ?? 0,
                        Payment_Date = DateTime.Now,
                        CardNumber = model.CardNumber,
                        CardHolderName = model.CardHolderName,
                        Payment_Method = reservations.FirstOrDefault()?.Payment_Method ?? "Kredi Kartı",
                        CVV = model.CVV,
                        Payment_Amount = model.TotalPrice,
                        Payment_Status = "Ödendi"
                    };

                    c.Payments.Add(payment);
                    c.SaveChanges();

                    // Rezervasyonları güncelle
                    foreach (var reservation in reservations)
                    {
                        reservation.PaymentID = payment.PaymentID;
                        reservation.Reservation_Status = "Onaylandı";
                        reservation.Payment_Status = "Ödendi";
                        c.Reservations.Update(reservation);
                    }

                    c.SaveChanges();

                    // Başarılı ödeme sonrası yönlendirme
                    TempData["SuccessMessage"] = "Ödeme işleminiz başarıyla tamamlanmıştır.";
                    return RedirectToAction("PaymentSuccess", new { paymentId = payment.PaymentID });
                }
                catch (Exception ex)
                {
                    // Hata detaylarını yazdır
                    Console.WriteLine($"KRİTİK HATA: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }

                    ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                    return RedirectToAction("PaymentSuccess",model);
                }
            }
        }

        [AllowAnonymous]
        public IActionResult PaymentSuccess(int paymentId)
        {
            if (paymentId > 0)
            {
                using (var c = new Context())
                {
                    // Önce ödemeyi al
                    var payment = c.Payments
                        .FirstOrDefault(p => p.PaymentID == paymentId);

                    if (payment != null)
                    {
                        // Sonra bu ödemeye ait tüm rezervasyonları getir
                        var reservations = c.Reservations
                            .Where(r => r.PaymentID == paymentId)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.DepartureAirport)
                            .Include(r => r.Flight)
                                .ThenInclude(f => f.ArrivalAirport)
                            .Include(r => r.Passenger)
                            .ToList();

                        // Ödeme bilgilerini ViewBag'e ekle
                        ViewBag.PaymentDate = payment.Payment_Date;
                        ViewBag.PaymentMethod = payment.Payment_Method;
                        ViewBag.TotalAmount = payment.Payment_Amount;

                        // Veya toplam tutarı rezervasyonlardan hesapla (eğer payment.Payment_Amount toplam değilse)
                        // ViewBag.TotalAmount = reservations.Sum(r => r.Amount); // veya ilgili tutar alanı

                        return View(new PaymentDetailsViewModel
                        {
                            Payment = payment,
                            Reservations = reservations
                        });
                    }
                }
            }

            // Eğer ödeme ID'si yoksa, kullanıcının son ödemesini göster
            // Benzer şekilde düzenleyin...

            return View(new PaymentDetailsViewModel
            {
                Payment = null,
                Reservations = new List<Reservation>()
            });
        }


        [AllowAnonymous]
        public IActionResult PaymentFailed()
        {
            return View();
        }

    }
}
