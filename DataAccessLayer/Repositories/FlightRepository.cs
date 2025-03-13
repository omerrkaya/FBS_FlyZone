using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

namespace DataAccessLayer.Repositories
{
    public class FlightRepository : IFlightDal
    {

        public void Add(Flight flight)
        {
            using var context = new Context();
            context.Add(flight);
            context.SaveChanges();
        }

        public void Delete(Flight flight)
        {
            using var context = new Context();
            context.Remove(flight);
            context.SaveChanges();
        }

        public List<Flight> GetAll()
        {
            using var context = new Context();
            return context.Flights.ToList();
        }

        public Flight GetById(int id)
        {
            using var context = new Context();
            return context.Flights.Find(keyValues: id) ?? throw new InvalidOperationException("Flight not found");
        }

        public void Update(Flight flight)
        {
            using var context = new Context();
            context.Update(flight);
            context.SaveChanges();
        }
    }
}
