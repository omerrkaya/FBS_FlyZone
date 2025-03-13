using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IAirportDal
    {
        List<Airport> GetAll();
        void Add(Airport airport);
        void Update(Airport airport);
        void Delete(Airport airport);
        Airport GetById(int id);
    }
}
