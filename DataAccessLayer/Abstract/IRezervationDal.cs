using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IReservationDal
    {
        List<Reservation> GetAll();
        void Add(Reservation rezervation);
        void Update(Reservation rezervation);
        void Delete(Reservation rezervation);
        Reservation GetById(int id);
    }
}
