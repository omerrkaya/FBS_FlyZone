using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IReservationService
    {
        void AddReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        void DeleteReservation(int reservationId);
        Reservation GetReservationById(int reservationId);
        List<Reservation> GetAllReservations();
         List<Reservation> GetReservationsByUserId(int userId);
    }
}
