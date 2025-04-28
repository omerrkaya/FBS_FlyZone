using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfSeatRepository : IGenericDal<Seat>, ISeatDal
    {
        public void Delete(Seat genent)
        {
           var c = new Context();
            var seat = c.Seats.FirstOrDefault(x => x.SeatID == genent.SeatID);
            if (seat != null)
            {
                c.Seats.Remove(seat);
                c.SaveChanges();
            }
        }

        public List<Seat> GetAll()
        {
            var c = new Context();
            var seats = c.Seats.ToList();
            if (seats == null || seats.Count == 0)
                throw new Exception("Koltuk bulunamadı."); // ya da özel bir exception sınıfı
            return seats;
        }

        public Seat GetById(int id)
        {
            var c = new Context();
            var seat = c.Seats.FirstOrDefault(x => x.SeatID == id);
            if (seat == null)
                throw new Exception("Koltuk bulunamadı."); // ya da özel bir exception sınıfı
            return seat;
        }

        public void Insert(Seat genent)
        {
            var c = new Context();
            c.Seats.Add(genent);
            c.SaveChanges();
        }

        public void Update(Seat genent)
        {
            var c = new Context();
            var seat = c.Seats.FirstOrDefault(x => x.SeatID == genent.SeatID);
            if (seat != null)
            {
                seat.IsOccupied = genent.IsOccupied;
                seat.PassengerName = genent.PassengerName;
                c.SaveChanges();
            }
        }
    }
}
