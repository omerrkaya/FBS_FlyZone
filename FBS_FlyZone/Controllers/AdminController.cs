using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using System.Linq;
using System;
using System.Collections.Generic;

namespace FBS_FlyZone.Controllers

// AdminController sınıfının içeriğini düzenliyorum. her Controller a AllowAnonymous eklemem gerekiyor test aşamasında, Güvenlik için AllowAnonymous kaldırılması gerekli. İlk başta Auth ile yapmak istedim ama hata mesajlarıyla karşılaştım. Düzenlenmesi gerekli.
// Burada kullanıcıların erişebileceği sayfalarının kontrollerini yapıyorum.

//ADMİN PANELİ ERİŞİM LİNKİ
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly UserManager _userManager;
        private readonly Context _context;

        public AdminController()
        {
            _userManager = new UserManager(new EfUserRepository());
            _context = new Context();
        }

        // Dashboard kısmına buranın sayesinde erişebilmeyi planlıyorum.
        [AllowAnonymous]
        public IActionResult Index()
        {
            try {
                // İstatistik verilerini hazırlıyorum.
                ViewBag.UserCount = _context.Users.Count();
                ViewBag.FlightCount = _context.Flights.Count();
                ViewBag.ReservationCount = _context.Reservations.Count();
                ViewBag.AirlineCount = _context.Airlines.Count();
                
                // Son 5 rezervasyonu alıyorum.
                var lastReservations = _context.Reservations.OrderByDescending(x => x.Reservation_Date).Take(5).ToList();
                return View(lastReservations);
            }
            catch (Exception ex) {
                // Hata durumunda boş bir dashboard gösteriyorum. Hata mesajını da göstermeye çalışıyorum. Burası test edilmesi gerekiyor.
                ViewBag.ErrorMessage = "Veritabanı işlemlerinde bir hata oluştu: " + ex.Message;
                ViewBag.UserCount = 0;
                ViewBag.FlightCount = 0;
                ViewBag.ReservationCount = 0;
                ViewBag.AirlineCount = 0;
                return View(new List<Reservation>());
            }
        }

        // Kullanıcı Yönetimi
        [AllowAnonymous]
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Kullanıcı Ekle
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        // Kullanıcı Ekleme işlemini yapıyorum.
        public IActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.RegisterationDate = System.DateTime.Now;
            _userManager.RegisterUserAdd(user);
            return RedirectToAction("Users");
        }

        // Kullanıcı Düzenleme işlemini yapıyorum.
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        // Kullanıcı Düzenleme işlemini yapıyorum.
        public IActionResult EditUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        // Kullanıcı Silme işlemini yapıyorum.
        [AllowAnonymous]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        // Uçuş Yönetimi işlemlerini yapıyorum.
        [AllowAnonymous]
        public IActionResult Flights()
        {
            var flights = _context.Flights.ToList(); // Uçuşları listeleyip view'e gönderiyorum.
            return View(flights);
        }

        // Rezervasyon Yönetimi işlemlerini yapıyorum.
        [AllowAnonymous]
        public IActionResult Reservations()
        {
            var reservations = _context.Reservations.ToList();
            return View(reservations);
        }

        // Havayolları Yönetimi işlemlerini yapıyorum.
        [AllowAnonymous]
        public IActionResult Airlines()
        {
            var airlines = _context.Airlines.ToList();
            return View(airlines);
        }

        // Havaalanları Yönetimi işlemlerini yapıyorum.
        [AllowAnonymous]
        public IActionResult Airports()
        {
            var airports = _context.Airports.ToList();
            return View(airports);
        }

        // Test sayfası - Sadece bağlantıyı test etmek için bir test sayfası oluşturdum. /Admin/Test sekmesine tıklayarak erişebilirsiniz.
        [AllowAnonymous]
        public IActionResult Test()
        {
            return View();
        }
    }
} 