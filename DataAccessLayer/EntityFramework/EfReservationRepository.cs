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
    public class EfReservationRepository : IGenericDal<Reservation>, IReservationDal
    {
        public void Delete(Reservation genent)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Reservation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Reservation genent)
        {
           using (var c = new Context())
            {
                c.Reservations.Add(genent);
                c.SaveChanges();
            }
        }

        public void Update(Reservation genent)
        {
            throw new NotImplementedException();
        }
    }
}
