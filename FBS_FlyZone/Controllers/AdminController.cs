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
using System.Globalization;

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
            _flightManager = new FlightManager(new EfFlightRepository());
            _userManager = new UserManager(new EfUserRepository());
            _context = new Context();
        }

        // Dashboard kısmına buranın sayesinde erişebilmeyi planlıyorum.
        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                // İstatistik verilerini hazırlıyorum.
                ViewBag.UserCount = _context.Users.Count();
                ViewBag.FlightCount = _context.Flights.Count();
                ViewBag.ReservationCount = _context.Reservations.Count();
                ViewBag.AirlineCount = _context.Airlines.Count();

                // Son 5 rezervasyonu alıyorum.
                var lastReservations = _context.Reservations.OrderByDescending(x => x.Reservation_Date).Take(5).ToList();
                return View(lastReservations);
            }
            catch (Exception ex)
            {
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
            //zaten yapmış listelemeyi sen sayfayı ayarla daha sonra da verileri sayfaya basmaya bak tamam
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddFlight()
        {
            // Havalimanlarını ve havayollarını listeleyip dropdownlarda kullanmak üzere gönderiyoruz
            var airports = _context.Airports.ToList();
            var airlines = _context.Airlines.ToList();

            ViewBag.Airports = new SelectList(airports, "AirportID", "Airport_Name");
            ViewBag.Airlines = new SelectList(airlines, "AirlineID", "Airlines_Name");

            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult AddFlight(Flight flight)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(flight);
        //    }

        //    _flightManager.AddFlight(flight);

        //    RedirectToAction("Flights");
        //}


        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditFlight(int id)
        {
            var flight = _flightManager.GetFlightById(id); // FlightManager'dan uçuşu alıyoruz
            if (flight == null)
            {
                return NotFound(); // Eğer uçuş bulunamazsa 404 döndürüyoruz
            }

            // Airports ve Airlines verilerini doğru şekilde ekleyelim
            ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "Airport_Name");
            ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "Airlines_Name");

            return View(flight); // Bulunan uçuş verisini view'a gönderiyoruz
        }


        //Uçuş silme işlemi
        [AllowAnonymous]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            _context.SaveChanges();
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
                .GroupBy(f => new { From = f.ArrivalAirport.Airport_Name, To = f.ArrivalAirport.AirportID })
                .Select(g => new { Route = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            // YENİ SORGU: En çok rezervasyon yapılan ayı hesaplama
            var reservationsByMonth = _context.Reservations
                .GroupBy(r => r.Reservation_Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            // En çok rezervasyon yapılan ay
            if (reservationsByMonth.Any())
            {
                int mostBookedMonth = reservationsByMonth.First().Month;
                ViewBag.MostBookedMonth = mostBookedMonth;
                ViewBag.MostBookedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mostBookedMonth);
                ViewBag.MostBookedMonthCount = reservationsByMonth.First().Count;
            }

            // Tüm ayların rezervasyon sayıları (ay numarası, rezervasyon sayısı)
            ViewBag.ReservationsByMonth = reservationsByMonth;

            // Ay isimlerini de gönderelim
            ViewBag.MonthNames = Enumerable.Range(1, 12)
                .Select(i => new {
                    MonthNumber = i,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)
                })
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