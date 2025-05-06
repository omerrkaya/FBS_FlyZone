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
using Microsoft.EntityFrameworkCore;
using FBS_FlyZone.Models;

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
        public IActionResult Users(string searchName, string role, DateTime? registrationDate)
        {
            var query = _context.Users.AsQueryable();
            
            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(u => u.User_Name_Surname.Contains(searchName)); //isim ve soyisim arıyorum
            }
            
            if (!string.IsNullOrEmpty(role)) // null veya boş değilse !string bu anlama gelir.
            {
                query = query.Where(u => u.UserRole == role); // Kullanıcı Rolü arıyorum
            }
            
            if (registrationDate.HasValue) // kayıt tarihi null değilse = hasvalue bu anlama gelir.
            {
                query = query.Where(u => u.RegisterationDate.Date == registrationDate.Value.Date); //kullanıcı kayıt tarihi arıyorum
            }
            
            var users = query.ToList();
            
            // Filtreleme değerlerini ViewBag'e ekleyelim ki form değerleri korunsun
            ViewBag.SearchName = searchName;
            ViewBag.Role = role;
            ViewBag.RegistrationDate = registrationDate?.ToString("yyyy-MM-dd");
            
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

        [AllowAnonymous]
        public IActionResult Flights(int page = 1)
        {
            int pageSize = 75;
            var allFlights = _flightManager.GetFlightListWithAirport();
            int totalFlights = allFlights.Count();
            int totalPages = (int)Math.Ceiling((double)totalFlights / pageSize);

            var flights = allFlights
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(flights);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddFlight()
        {

            var results = _context.Flights
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Airline)
                .Include(f => f.Aircraft)
                .ToList();

            var model = new AddFlightModel
            {
                Flights = results
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddFlight(Flight flight)
        {

            if (!ModelState.IsValid)
            {
                return View(flight);
            }

            _flightManager.AddFlight(flight);

            return RedirectToAction("Flights");
        }


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


        [AllowAnonymous]
        public IActionResult Reservations(DateTime? reservationDate, string seatNumber, string paymentStatus, string reservationStatus)
        {
            var query = _context.Reservations
                .Include(r => r.Flight)
                .Include(r => r.Passenger)
                .AsQueryable();

            // Sadece Reservation tablosuna ait filtreler
            if (reservationDate.HasValue)
            {
                query = query.Where(r => r.Reservation_Date.Date == reservationDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(seatNumber))
            {
                query = query.Where(r => r.Seat_Number.Contains(seatNumber));
            }

            if (!string.IsNullOrEmpty(paymentStatus))
            {
                query = query.Where(r => r.Payment_Status == paymentStatus);
            }

            if (!string.IsNullOrEmpty(reservationStatus))
            {
                query = query.Where(r => r.Reservation_Status == reservationStatus);
            }

            var reservations = query.ToList();

            ViewBag.ReservationDate = reservationDate?.ToString("yyyy-MM-dd");
            ViewBag.SeatNumber = seatNumber;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.ReservationStatus = reservationStatus;

            return View(reservations);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditReservation(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(x => x.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.Airlines = new SelectList(_context.Airlines.ToList(), "AirlineID", "Airlines_Name");
            ViewBag.Airports = new SelectList(_context.Airports.ToList(), "AirportID", "Airport_Name");
            ViewBag.Flights = new SelectList(_context.Flights.ToList(), "FlightID", "Flight_Code");
            ViewBag.Passengers = new SelectList(_context.Passengers.ToList(), "PassengerID", "Passenger_Name_Surname");

        

            return View(reservation);
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
        public IActionResult Airports(string searchName, string searchIATA, string searchCity, string searchCountry)
        {
            var query = _context.Airports.AsQueryable();
            
            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(a => a.Airport_Name.Contains(searchName));
            }
            
            if (!string.IsNullOrEmpty(searchIATA))
            {
                query = query.Where(a => a.IATA_Code.Contains(searchIATA));
            }
            
            if (!string.IsNullOrEmpty(searchCity))
            {
                query = query.Where(a => a.AP_City.Contains(searchCity));
            }
            
            if (!string.IsNullOrEmpty(searchCountry))
            {
                query = query.Where(a => a.AP_Country.Contains(searchCountry));
            }
            
            var airports = query.ToList();
            
            // Filtreleme değerlerini ViewBag'e ekleyelim ki form değerleri korunsun
            ViewBag.SearchName = searchName;
            ViewBag.SearchIATA = searchIATA;
            ViewBag.SearchCity = searchCity;
            ViewBag.SearchCountry = searchCountry;
            
            return View(airports);
        }
      


        
        // Havaalanı Ekleme - POST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddAirport(Airport airport) // havaalanı ekleme
        {
            if (ModelState.IsValid) // model geçerli değilse
            {
                _context.Airports.Add(airport); // Havaalanını ekliyoruz
                _context.SaveChanges(); // Değişiklikleri kaydediyoruz
                return RedirectToAction("Airports"); // Havaalanları sayfasına yönlendiriyoruz
            }
            
            return RedirectToAction("Airports");
        }
        
        // Havaalanı Düzenleme - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditAirport(int id) // Havaalanı düzenleme işlemi
        {
            var airport = _context.Airports.Find(id); // Havaalanını buluyoz
            if (airport == null) // Eğer havaalanı bulunamazsa
            {
                return NotFound();  // 404 sayfasına yönlendiriyoz
            }
            
            return View(airport); // Havaalanı düzenleme sayfasını gösteriyoruz
        }
        
        // Havaalanı Düzenleme - POST ile yapılır bu şekilde düşündüm.
        [HttpPost]
        [AllowAnonymous]
        public IActionResult EditAirport(Airport airport)
        {
            if (ModelState.IsValid)
            {
                _context.Airports.Update(airport); // Update metodu ile güncelleniyo
                _context.SaveChanges(); // Değişiklikleri kaydediyoz
                return RedirectToAction("Airports"); // Havaalanları sayfasına yönlendiriyoz
            }
            
            return View(airport); // Eğer model geçerli değilse, düzenleme sayfasını tekrar gösteriyoruz
        }
        
        // Havaalanı Silme, 
        [AllowAnonymous]
        public IActionResult DeleteAirport(int id) // Havaalanı silme işlemi
        {
            var airport = _context.Airports.Find(id); // Havaalanını buluyoz
            if (airport == null) // Eğer havaalanı bulunamazsa
            {
                return NotFound(); // 404 sayfasına yönlendiriyoz
            }
            
            _context.Airports.Remove(airport); // Havaalanını siliyoz
            _context.SaveChanges();
            return RedirectToAction("Airports"); // Havaalanları sayfasına yönlendiriyoz
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

        // Rota karşılaştırma API endpoint'i
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CompareRoutes(string route1, string route2, string metric = "all")
        {
            // Rota parametrelerini ayrıştır
            var route1Parts = route1?.Split('-');
            var route2Parts = route2?.Split('-');

            if (route1Parts?.Length != 2 || route2Parts?.Length != 2)
            {
                return Json(new { success = false, message = "Geçersiz rota formatı" });
            }

            // Rota verilerini al
            var route1Data = GetRouteData(route1Parts[0], route1Parts[1]);
            var route2Data = GetRouteData(route2Parts[0], route2Parts[1]);

            // Karşılaştırma verilerini hazırla
            var comparisonData = new
            {
                success = true,
                labels = new[] { "Gelir (TL)", "Doluluk Oranı (%)", "Müşteri Memnuniyeti", "İptal Oranı (%)" },
                datasets = new[]
                {
                    new
                    {
                        label = GetRouteLabel(route1Parts[0], route1Parts[1]),
                        data = route1Data,
                        backgroundColor = "rgba(54, 162, 235, 0.7)",
                        borderColor = "rgb(54, 162, 235)",
                        borderWidth = 1
                    },
                    new
                    {
                        label = GetRouteLabel(route2Parts[0], route2Parts[1]),
                        data = route2Data,
                        backgroundColor = "rgba(255, 99, 132, 0.7)",
                        borderColor = "rgb(255, 99, 132)",
                        borderWidth = 1
                    }
                }
            };

            return Json(comparisonData);
        }

        // Rota verilerini getiren yardımcı metod
        private double[] GetRouteData(string from, string to)
        {
            // Gerçek uygulamada burada veritabanından ilgili rotaya ait gerçek verileri çekeceksiniz
            // Şimdilik demo amaçlı rastgele veriler üretiyoruz
            
            // Metriklerin tutarlı olması için, belirli rota kombinasyonları için sabit değerler döndürelim
            Random random = new Random();
            
            // Format: [Gelir, Doluluk Oranı, Müşteri Memnuniyeti, İptal Oranı]
            switch ($"{from}-{to}")
            {
                case "ist-ank":
                    return new double[] { 1200000, 85, 4.5, 3 };
                case "ist-izm":
                    return new double[] { 950000, 78, 4.2, 4 };
                case "ist-ant":
                    return new double[] { 1100000, 82, 4.3, 2 };
                case "ank-izm":
                    return new double[] { 870000, 75, 4.1, 5 };
                case "ank-ant":
                    return new double[] { 920000, 80, 4.4, 3 };
                default:
                    // Diğer rotalar için rastgele değerler
                    return new double[] { 
                        random.Next(800000, 1300000),  // Gelir: 800,000 - 1,300,000 TL arası
                        random.Next(65, 90),           // Doluluk: %65-90 arası
                        Math.Round(random.NextDouble() * (4.8 - 3.9) + 3.9, 1),  // Memnuniyet: 3.9-4.8 arası
                        random.Next(1, 8)              // İptal: %1-8 arası
                    };
            }
        }

        // Rota etiketi için yardımcı metod
        private string GetRouteLabel(string from, string to)
        {
            var airports = new Dictionary<string, string>
            {
                { "ist", "İstanbul" },
                { "ank", "Ankara" },
                { "izm", "İzmir" },
                { "ant", "Antalya" },
                { "adb", "İzmir" },
                { "esb", "Ankara" },
                { "saw", "İstanbul Sabiha Gökçen" },
                { "ayt", "Antalya" }
            };

            string fromName = airports.ContainsKey(from.ToLower()) ? airports[from.ToLower()] : from.ToUpper();
            string toName = airports.ContainsKey(to.ToLower()) ? airports[to.ToLower()] : to.ToUpper();
            
            return $"{fromName} - {toName}";
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
                .Select(i => new
                {
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
