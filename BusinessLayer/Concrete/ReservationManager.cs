using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete
{
    public class ReservationManager : IReservationService
    {
        IReservationDal _reservationDal;

        public ReservationManager(IReservationDal reservationDal)
        {
            _reservationDal = reservationDal;
        }

        public void AddReservation(Reservation reservation)
        {
            _reservationDal.Insert(reservation);
        }

        public void DeleteReservation(int reservationId)
        {
            _reservationDal.Delete(new Reservation { ReservationID = reservationId });
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationDal.GetAll();
        }

        public List<decimal> GetMonthlyRevenueFromPayments(int year)
        {
            {
                // Ocak'tan Aralık'a kadar 12 ay için sıfırla dolu liste
                var monthlyRevenue = Enumerable.Repeat(0m, 12).ToList();

                using (var _context = new Context())
                {
                    // Belirtilen yıla ait ödemeleri gruplayıp toplam geliri hesapla
                    var payments = _context.Payments
                        .Where(p => p.Payment_Date.Year == year)
                        .GroupBy(p => p.Payment_Date.Month)
                        .Select(g => new
                        {
                            Month = g.Key,
                            Total = g.Sum(p => p.Payment_Amount)
                        })
                        .ToList();
                    // Her ayın toplam gelirini ilgili index'e yerleştir (Ocak: 0, Şubat: 1, ...)
                    foreach (var item in payments)
                    {
                        monthlyRevenue[item.Month - 1] = item.Total;
                    }

                }


                return monthlyRevenue;
            }
        }

        public Reservation GetReservationById(int reservationId)
        {

            using (var context = new Context())
            {
                return context.Reservations.FirstOrDefault(r => r.ReservationID == reservationId);
            }
        }

        public Reservation GetReservationByPassengerAndFlight(int passengerId, int flightId)
        {
            return _reservationDal.GetReservationByPassengerAndFlight(passengerId, flightId);
        }

        public List<Reservation> GetReservationsByUserId(int userId)
        {
            using (var c = new Context())
            {
                return c.Reservations.Where(r => r.UserID == userId).ToList();
            }
        }

        public void UpdateReservation(Reservation reservation)
        {
            _reservationDal.Update(reservation);
        }
    }
}
