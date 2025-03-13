using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IAirlineDal
    {
        List<Airline> GetAll();
        void Add(Airline airline);
        void Update(Airline airline);
        void Delete(Airline airline);
        Airline GetById(int id);
    }
}
