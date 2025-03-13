using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfAirportRepository : IGenericDal<Airport>, IAirportDal
    {
        public void Delete(Airport genent)
        {
            throw new NotImplementedException();
        }

        public List<Airport> GetAll()
        {
            throw new NotImplementedException();
        }

        public Airport GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Airport genent)
        {
            throw new NotImplementedException();
        }

        public void Update(Airport genent)
        {
            throw new NotImplementedException();
        }
    }
}
