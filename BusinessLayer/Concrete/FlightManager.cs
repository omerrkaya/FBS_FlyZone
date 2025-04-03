using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete
{
    public class FlightManager : IFlightService
    {

        EfFlightRepository efFlightRepository;

        public FlightManager(EfFlightRepository efFlightRepository)
        {
            efFlightRepository = new EfFlightRepository();
        }

        public void AddFlight(Flight flight)
        {
            efFlightRepository.Insert(flight);
        }

        public void DeleteFlight(Flight flight)
        {
            efFlightRepository.Delete(flight);
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
            return efFlightRepository.GetById(id);
        }

        public void UpdateFlight(Flight flight)
        {
            efFlightRepository.Update(flight);
        }

    }
}
