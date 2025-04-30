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
                    .Max(r => r.Reservation_Date).Date;

                if (latestReservationDate == default)
                {
                    // Rezervasyon bulunamadı
                    return RedirectToAction("Index", "Home");
                }

                // Son rezervasyonları getir
                var reservations = c.Reservations
                    .Where(r => r.UserID == int.Parse(userId) &&
                               r.Reservation_Date.Date == latestReservationDate &&
                               r.Reservation_Status == "Onay Bekliyor")
                    .Include(r => r.Passenger)
                    .Include(r => r.Flight).Include(r => r.Flight.Airline).Include(r => r.Flight.DepartureAirport).Include(r => r.Flight.ArrivalAirport)
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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
                    .Max(r => r.Reservation_Date);

                // Rezervasyonları getir
                var reservations = c.Reservations
                    .Where(r => r.UserID == int.Parse(userId) &&
                               r.Reservation_Date == latestReservationDate &&
                               r.Reservation_Status == "Onay Bekliyor")
                    .Include(r => r.Flight)
                    .ToList();

                // Transaction başlat
                using (var transaction = c.Database.BeginTransaction())
                {
                    try
                    {
                        // Toplam tutarı hesapla
                        decimal totalAmount = 0;
                        foreach (var reservation in reservations)
                        {
                            totalAmount += reservation.Flight.Flight_Price;

                            // Rezervasyon durumunu güncelle
                            reservation.Reservation_Status = "Onaylandı";
                            reservation.Payment_Method = "Kredi Kartı";
                            c.Update(reservation);

                            // Ödeme kaydı oluştur
                            var payment = new Payment
                            {
                                ReservationID = reservation.ReservationID,
                                Payment_Date = DateTime.Now,
                                Payment_Amount = reservation.Flight.Flight_Price,
                                Payment_Method = "Kredi Kartı",
                                Payment_Status = "Tamamlandı"
                            };
                            c.Payments.Add(payment);
                        }

                        c.SaveChanges();
                        transaction.Commit();

                        TempData["SuccessMessage"] = "Ödemeniz başarıyla alındı. Biletiniz mail adresinize gönderildi.";
                        return RedirectToAction("PaymentSuccess");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Ödeme işlemi sırasında bir hata oluştu: " + ex.Message);
                        return View(model);
                    }
                }
            }
        }

        [AllowAnonymous]
        public IActionResult PaymentSuccess()
        {
            // Kullanıcının son ödeme bilgilerini getir
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                using (var c = new Context())
                {
                    var latestPayments = c.Payments
                        .Where(p => p.Reservation.UserID == int.Parse(userId) && p.Payment_Status == "Tamamlandı")
                        .OrderByDescending(p => p.Payment_Date)
                        .Take(5)
                        .Include(p => p.Reservation)
                        .ThenInclude(r => r.Flight)
                        .Include(p => p.Reservation.Passenger)
                        .ToList();

                    return View(latestPayments);
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}
