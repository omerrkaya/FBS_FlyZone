using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IFlightDal
    {
        List<Flight> GetAll();
        void Add(Flight flight);
        void Update(Flight flight);
        void Delete(Flight flight);
        Flight GetById(int id);
    }
}
