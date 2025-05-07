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
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;

namespace FBS_FlyZone.Controllers


//ADMİN PANELİ ERİŞİM LİNKİ
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly UserManager _userManager;
        private readonly Context _context;
        private readonly FlightManager _flightManager;
        private readonly ReservationManager _reservationManager;

        public AdminController()
        {
            _flightManager = new FlightManager(new EfFlightRepository());
            _userManager = new UserManager(new EfUserRepository());
            _reservationManager = new ReservationManager(new EfReservationRepository());
            _context = new Context();
        }

        // Dashboard kısmına buranın sayesinde erişebilmeyi planlıyorum.
        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                var reservationsByMonth = _context.Reservations
                .GroupBy(r => r.Reservation_Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

                if (reservationsByMonth.Any())
                {
                    int mostBookedMonth = reservationsByMonth.First().Month;
                    ViewBag.MostBookedMonth = mostBookedMonth;
                    ViewBag.MostBookedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mostBookedMonth);
                    ViewBag.MostBookedMonthCount = reservationsByMonth.First().Count;
                }

                ViewBag.ReservationsByMonth = reservationsByMonth.ToDictionary(x => x.Month, x => x.Count);


                // İstatistik verilerini hazırlıyorum.
                ViewBag.UserCount = _context.Users.Count();
                ViewBag.FlightCount = _context.Flights.Count();
                ViewBag.ReservationCount = _context.Reservations.Count();
                ViewBag.AirlineCount = _context.Airlines.Count();

                // Son 5 rezervasyonu alıyorum.
                var lastReservations = _context.Reservations.OrderByDescending(x => x.Reservation_Date)
                    .Take(5)
                    .Include("Flight")
                    .Include("Passenger")
                    .ToList();
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
        public IActionResult Users(string searchName, string role, DateTime? registrationDate, int page = 1)
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

            int pageSize = 10;


            var users = query.Skip((page - 1) * pageSize)
           .Take(pageSize).ToList();

            int totalPages = (int)Math.Ceiling((double)query.Count() / pageSize);

            // Filtreleme değerlerini ViewBag'e ekleyelim ki form değerleri korunsun
            ViewBag.SearchName = searchName;
            ViewBag.Role = role;
            ViewBag.RegistrationDate = registrationDate?.ToString("yyyy-MM-dd");
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Flights(string flightcode, string departure, string arrival, DateTime? flightDate, int page = 1)
        {
            var query = _context.Flights.AsQueryable();
            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(flightcode))
            {
                query = query.Where(u => u.Flight_Code.Contains(flightcode)); //isim ve soyisim arıyorum
            }

            if (!string.IsNullOrEmpty(departure)) // null veya boş değilse !string bu anlama gelir.
            {
                query = query.Where(u => u.DepartureAirport.Airport_Name == departure); // Kullanıcı Rolü arıyorum
            }

            if (!string.IsNullOrEmpty(arrival)) // null veya boş değilse !string bu anlama gelir.
            {
                query = query.Where(u => u.ArrivalAirport.Airport_Name == arrival); // Kullanıcı Rolü arıyorum
            }

            if (flightDate.HasValue) // kayıt tarihi null değilse = hasvalue bu anlama gelir.
            {
                query = query.Where(u => u.Flight_DateTime.Date == flightDate.Value.Date); //kullanıcı kayıt tarihi arıyorum
            }

            int pageSize = 75;
            int totalFlights = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalFlights / pageSize);

            var flights = query.Include("DepartureAirport").Include("ArrivalAirport").Include("Airline").Include("Aircraft")
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

        // Havayolu Ekleme - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddAirline()
        {
            return View();
        }

        // Havayolu Ekleme - POST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddAirline(Airline airline)
        {
            if (ModelState.IsValid)
            {
                _context.Airlines.Add(airline);
                _context.SaveChanges();
                return RedirectToAction("Airlines");
            }

            return View(airline);
        }

        // Havayolu Düzenleme - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditAirline(int id)
        {
            var airline = _context.Airlines.Find(id);
            if (airline == null)
            {
                return NotFound();
            }

            return View(airline);
        }

        // Havayolu Düzenleme - POST
        [HttpPost]
        [AllowAnonymous]
        public IActionResult EditAirline(Airline airline)
        {
            if (ModelState.IsValid)
            {
                _context.Airlines.Update(airline);
                _context.SaveChanges();
                return RedirectToAction("Airlines");
            }

            return View(airline);
        }

        // Havayolu Silme
        [AllowAnonymous]
        public IActionResult DeleteAirline(int id)
        {
            var airline = _context.Airlines.Find(id);
            if (airline == null)
            {
                return NotFound();
            }

            _context.Airlines.Remove(airline);
            _context.SaveChanges();
            return RedirectToAction("Airlines");
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





        private double[] GetRouteData(string fromCode, string toCode)
        {
            // 1. Rotalara uyan uçuşları getir
            var flights = _context.Flights
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Where(f => f.DepartureAirport.IATA_Code == fromCode && f.ArrivalAirport.IATA_Code == toCode)
                .ToList();

            if (!flights.Any())
                return new double[] { 0, 0, 0, 0 };

            var flightIds = flights.Select(f => f.FlightID).ToList();

            var reservations = _context.Reservations
                .Where(r => flightIds.Contains(r.FlightID) && r.Payment_Status == "Ödendi")
                .ToList();

            decimal totalRevenue = reservations.Sum(r => r.Flight.Flight_Price);

            var avgOccupancy = flights.Average(f =>
            {
                int reservedSeats = reservations.Count(r => r.FlightID == f.FlightID);
                return (double)reservedSeats / 100 * 100; // kapasite = 100
            });

            // 6. İptal oranı (%)
            double cancelRate = (double)reservations.Count(r => r.Reservation_Status == "Onay Bekliyor") / reservations.Count * 100;

            // 7. Müşteri memnuniyeti pas — 0 döndürüyoruz
            double dummyRating = 0;

            // 8. Veriyi döndür
            return new double[]
            {
        (double)totalRevenue,
        Math.Round(avgOccupancy, 1),
        dummyRating,
        Math.Round(cancelRate, 1)
            };
        }

        private string GetRouteLabel(string fromCode, string toCode)
        {
            var airportNames = _context.Airports
                .Where(a => a.IATA_Code.ToLower() == fromCode.ToLower() || a.IATA_Code.ToLower() == toCode.ToLower())
                .ToDictionary(a => a.IATA_Code.ToLower(), a => a.Airport_Name);

            string fromName = airportNames.ContainsKey(fromCode.ToLower())
                ? airportNames[fromCode.ToLower()]
                : fromCode.ToUpper();

            string toName = airportNames.ContainsKey(toCode.ToLower())
                ? airportNames[toCode.ToLower()]
                : toCode.ToUpper();

            return $"{fromName} - {toName}";
        }


        // Raporlar sayfası - Yeni eklendi
        [AllowAnonymous]
        public IActionResult Reports()
        {

            var revenue2025 = _reservationManager.GetMonthlyRevenueFromPayments(2025);

            ViewBag.MonthlyRevenue = revenue2025;

            var months = new[]
                    {
                        "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                        "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
                     };
            ViewBag.Months = months;

            var monthlyReservations = _context.Reservations
                .GroupBy(r => r.Reservation_Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToList();

            int[] monthlyCounts = new int[12];

            foreach (var item in monthlyReservations)
            {
                monthlyCounts[item.Month - 1] = item.Count;
            }

            ViewBag.MonthlyReservations = monthlyCounts;


            var avgPrice = _context.Flights.Average(x => x.Flight_Price);

            decimal avg = avgPrice;

            var seats = _context.Seats.ToList();

            var IsOccupiedSeats = seats.Where(x => x.IsOccupied).Count();


            decimal occupiedSeatsPercentage = (decimal)IsOccupiedSeats / seats.Count * 100;
            ViewBag.IsOccuipedSeats = occupiedSeatsPercentage; // Doğrudan float olarak atıyoruz





            // Havayollarına göre rezervasyon dağılımı
            Dictionary<string, int> airlineStats = new Dictionary<string, int>();

            var airlineReservations = _context.Reservations
                .Join(_context.Flights, r => r.FlightID, f => f.FlightID, (r, f) => new { r, f })
                .Join(_context.Airlines, rf => rf.f.AirlineID, a => a.AirlineID, (rf, a) => new { rf.r, rf.f, a })
                .ToList()
                .GroupBy(x => x.a.Airlines_Name)
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
                .GroupBy(f => new { From = f.DepartureAirport.Airport_Name, To = f.ArrivalAirport.Airport_Name })
                .Select(g => new { Route = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();


            var reservationsByMonth = _context.Reservations
                .GroupBy(r => r.Reservation_Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            if (reservationsByMonth.Any())
            {
                int mostBookedMonth = reservationsByMonth.First().Month;
                ViewBag.MostBookedMonth = mostBookedMonth;
                ViewBag.MostBookedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mostBookedMonth);
                ViewBag.MostBookedMonthCount = reservationsByMonth.First().Count;
            }

            ViewBag.ReservationsByMonth = reservationsByMonth;


          

            var model = new AdminReportsModel
            {
                yuzdelikDoluluk = occupiedSeatsPercentage,
                yuzdelikAvg = avg
            };
            return View(model);
        }

        // Test sayfası - Sadece bağlantıyı test etmek için bir test sayfası oluşturdum. /Admin/Test sekmesine tıklayarak erişebilirsiniz.
        [AllowAnonymous]
        public IActionResult Test()
        {
            return View();
        }
    }


}
