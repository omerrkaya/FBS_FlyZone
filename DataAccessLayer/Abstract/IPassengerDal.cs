using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IPassengerDal
    {
        List<Passenger> GetAll();
        void Add(Passenger passenger);
        void Update(Passenger passenger);
        void Delete(Passenger passenger);
        Passenger GetById(int id);
    }
}
