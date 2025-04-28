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
    public class SeatManager : ISeatService
    {
        ISeatDal _seatDal;

        public SeatManager(ISeatDal seatDal)
        {
            _seatDal = seatDal;
        }

        public List<Seat> GetAll()
        {
            return _seatDal.GetAll();
        }

        public List<Seat> GetAvailableSeatsByFlightId(int flightId)
        {
            throw new NotImplementedException();
        }

        public Seat GetById(int id)
        {
            return _seatDal.GetById(id);
        }

        public List<Seat> GetOccupiedSeatsByFlightId(int flightId)
        {
            throw new NotImplementedException();
        }

        public Seat GetSeatBySeatNumber(string seat)
        {
            var c = new Context();

            return c.Seats.FirstOrDefault(x => x.SeatNumber == seat);

        }

        public void SeatAdd(Seat seat)
        {
            _seatDal.Insert(seat);
        }

        public void SeatDelete(Seat seat)
        {
            _seatDal.Delete(seat);
        }

        public void SeatUpdate(Seat seat)
        {
            _seatDal.Update(seat);
        }
    }
}
