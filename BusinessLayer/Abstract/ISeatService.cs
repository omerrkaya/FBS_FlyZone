using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface ISeatService
    {
        void SeatAdd(Seat seat);
        void SeatDelete(Seat seat);
        void SeatUpdate(Seat seat);
        List<Seat> GetAll();
        Seat GetById(int id);
        List<Seat> GetOccupiedSeatsByFlightId(int flightId);
        List<Seat> GetAvailableSeatsByFlightId(int flightId);

        Seat GetSeatBySeatNumber(string seat);
    }
}
