using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly FlightManager _flightManager;

        public AdminController()
        {
            _flightManager= new FlightManager(new EfFlightRepository());
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
            var flights = _flightManager.GetFlightListWithAirport(); // Uçuşları listeleyip view'e gönderiyorum.
            return View(flights);
        }
        
        // Yeni Uçuş Ekleme - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddFlight()
        {
            // Havayolları listesi
            ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "AirlineName");
            
            // Havaalanları listesi
            ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "AirportName");
            
            // Uçak listesi
            ViewBag.Aircrafts = new SelectList(_context.Aircrafts.ToList(), "AircraftID", "AircraftName");
            
            return View();
        }
        
        // Yeni Uçuş Ekleme - POST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddFlight(Flight flight)
        {
            if (!ModelState.IsValid)
            {
                // Form doğrulama hataları durumunda seçim listelerini tekrar doldur
                ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "AirlineName");
                ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "AirportName");
                ViewBag.Aircrafts = new SelectList(_context.Aircrafts.ToList(), "AircraftID", "AircraftName");
                return View(flight);
            }
            
            // Uçuşu veritabanına ekle
            _context.Flights.Add(flight);
            _context.SaveChanges();
            
            TempData["SuccessMessage"] = "Uçuş başarıyla eklendi.";
            return RedirectToAction("Flights");
        }
        
        // Uçuş Düzenleme - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditFlight(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            
            // Havayolları listesi
            ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "AirlineName", flight.AirlineID);
            
            // Havaalanları listesi
            ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "AirportName");
            
            // Uçak listesi
            ViewBag.Aircrafts = new SelectList(_context.Aircrafts.ToList(), "AircraftID", "AircraftName", flight.AircraftID);
            
            return View(flight);
        }
        
        // Uçuş Düzenleme - POST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult EditFlight(Flight flight)
        {
            if (!ModelState.IsValid)
            {
                // Form doğrulama hataları durumunda seçim listelerini tekrar doldur
                ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "AirlineName", flight.AirlineID);
                ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "AirportName");
                ViewBag.Aircrafts = new SelectList(_context.Aircrafts.ToList(), "AircraftID", "AircraftName", flight.AircraftID);
                return View(flight);
            }
            
            // Uçuşu veritabanında güncelle
            _context.Flights.Update(flight);
            _context.SaveChanges();
            
            TempData["SuccessMessage"] = "Uçuş başarıyla güncellendi.";
            return RedirectToAction("Flights");
        }
        
        // Uçuş Silme
        [AllowAnonymous]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            
            // Uçuşla ilişkili rezervasyonları kontrol et
            var hasReservations = _context.Reservations.Any(r => r.FlightID == id);
            if (hasReservations)
            {
                TempData["ErrorMessage"] = "Bu uçuşa ait rezervasyonlar olduğu için silinemez.";
                return RedirectToAction("Flights");
            }
            
            // Uçuşu sil
            _context.Flights.Remove(flight);
            _context.SaveChanges();
            
            TempData["SuccessMessage"] = "Uçuş başarıyla silindi.";
            return RedirectToAction("Flights");
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
        
        // Sistem Ayarları sayfası - Yeni eklendi
        [AllowAnonymous]
        public IActionResult Settings()
        {
            // Sistem ayarları için gerekli verileri yükle
            ViewBag.SiteTitle = "FlyZone Uçuş Rezervasyon Sistemi";
            ViewBag.SiteDescription = "Kolay ve hızlı uçak bileti rezervasyon sistemi";
            ViewBag.ContactEmail = "info@flyzone.com";
            ViewBag.SupportPhone = "+90 212 555 0000";
            
            return View();
        }
        
        // Raporlar sayfası - Yeni eklendi
        [AllowAnonymous]
        public IActionResult Reports()
        {
            // Raporlama için gerekli sayısal verileri ViewBag ile gönderiyoruz
            
            // Aylık rezervasyon sayıları
            ViewBag.MonthlyReservations = new int[] { 65, 59, 80, 81, 56, 55, 40, 45, 60, 49, 52, 50 };
            
            // Havayollarına göre rezervasyon dağılımı
            Dictionary<string, int> airlineStats = new Dictionary<string, int>();
            
            // Fix: Use ToList() before grouping to move query execution to client side,
            // and use Airlines_Name instead of AirlineName
            var airlineReservations = _context.Reservations
                .Join(_context.Flights, r => r.FlightID, f => f.FlightID, (r, f) => new { r, f })
                .Join(_context.Airlines, rf => rf.f.AirlineID, a => a.AirlineID, (rf, a) => new { rf.r, rf.f, a })
                .ToList() // Execute the query before grouping
                .GroupBy(x => x.a.Airlines_Name) // Use the actual database field name
                .Select(g => new { AirlineName = g.Key, Count = g.Count() })
                .ToList();
                
            foreach (var stat in airlineReservations)
            {
                airlineStats.Add(stat.AirlineName, stat.Count);
            }
            ViewBag.AirlineStats = airlineStats;
            
            // Popüler rotalar
            ViewBag.TopRoutes = _context.Flights
                .Join(_context.Reservations, f => f.FlightID, r => r.FlightID, (f, r) => f)
                .GroupBy(f => new { From = f.DepartureAirportID, To = f.ArrivalAirportID })
                .Select(g => new { Route = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();
            
            return View();
        }

        // Test sayfası - Sadece bağlantıyı test etmek için bir test sayfası oluşturdum. /Admin/Test sekmesine tıklayarak erişebilirsiniz.
        [AllowAnonymous]
        public IActionResult Test()
        {
            return View();
        }
    }
}