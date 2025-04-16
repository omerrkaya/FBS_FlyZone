using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfPassengerRepository : IGenericDal<Passenger>, IPassengerDal
    {
        public void Delete(Passenger genent)
        {
            throw new NotImplementedException();
        }

        public List<Passenger> GetAll()
        {
            throw new NotImplementedException();
        }

        public Passenger GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Passenger genent)
        {
            using (var c = new Context())
            {
                c.Passengers.Add(genent);
                c.SaveChanges();
            }
        }

        public void Update(Passenger genent)
        {
            throw new NotImplementedException();
        }
    }
}
