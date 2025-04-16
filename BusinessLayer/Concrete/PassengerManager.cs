using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class PassengerManager : IPassengerService
    {
        IPassengerDal _passengerDal;

        public PassengerManager(IPassengerDal passengerDal)
        {
            _passengerDal = passengerDal;
        }

        public void AddPassenger(Passenger passenger)
        {
            _passengerDal.Insert(passenger);
        }

        public void DeletePassenger(int passengerId)
        {
            _passengerDal.Delete(new Passenger { PassengerID = passengerId });
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengerDal.GetAll();
        }

        public Passenger GetPassengerById(int passengerId)
        {
            return _passengerDal.GetById(passengerId);
        }

        public void UpdatePassenger(Passenger passenger)
        {
            throw new NotImplementedException();
        }
    }
}
