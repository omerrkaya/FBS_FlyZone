using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfAirlineRepository : IGenericDal<Airline>, IAirlineDal
    {
        public void Delete(Airline genent)
        {
            throw new NotImplementedException();
        }

        public List<Airline> GetAll()
        {
            throw new NotImplementedException();
        }

        public Airline GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Airline genent)
        {
            throw new NotImplementedException();
        }

        public void Update(Airline genent)
        {
            throw new NotImplementedException();
        }
    }
}
