using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete
{
    public class FlightManager : IFlightService
    {

        IFlightDal _flightDal;

        public FlightManager(IFlightDal flightDal)
        {
            _flightDal = flightDal;
        }

        public void AddFlight(Flight flight)
        {
            _flightDal.Insert(flight);
        }

        public void DeleteFlight(Flight flight)
        {
            _flightDal.Delete(flight);
        }

        public List<Flight> GetListAll()
        {
            using var c = new Context();
            return c.Set<Flight>()
                    .Include(f => f.Airline) // İlgili veriyi de yükle
                    .Include(f => f.DepartureAirport) // İlgili veriyi de yükle
                    .Include(f => f.ArrivalAirport)
                    .ToList();
        }


        public Flight GetFlightById(int id)
        {
            return _flightDal.GetById(id);
        }

        public void UpdateFlight(Flight flight)
        {
            _flightDal.Update(flight);
        }

        public List<Flight> GetFlightListWithAirport()
        {
            return _flightDal.GetFlightListWithAirport();
        }
    }
}
