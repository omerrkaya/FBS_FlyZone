using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfFlightRepository : IGenericDal<Flight>, IFlightDal
    {
        public void Delete(Flight genent)
        {
            throw new NotImplementedException();
        }

        public List<Flight> GetAll()
        {
            throw new NotImplementedException();
        }

        public Flight GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Flight genent)
        {
            throw new NotImplementedException();
        }

        public void Update(Flight genent)
        {
            throw new NotImplementedException();
        }
    }
}
