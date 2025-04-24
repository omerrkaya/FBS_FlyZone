using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

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
            using ( var c = new Context())
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
