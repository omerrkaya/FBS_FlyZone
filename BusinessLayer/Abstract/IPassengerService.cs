using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IPassengerService
    {
        // Add methods for passenger management
        void AddPassenger(Passenger passenger);
        void UpdatePassenger(Passenger passenger);
        void DeletePassenger(int passengerId);
        Passenger GetPassengerById(int passengerId);
        List<Passenger> GetAllPassengers();
    }
}
