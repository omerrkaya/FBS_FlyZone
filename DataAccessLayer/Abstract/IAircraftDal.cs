using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IAircraftDal
    {
        List<Aircraft> GetAll();
        void Add(Aircraft aircraft);
        void Update(Aircraft aircraft);
        void Delete(Aircraft aircraft);
        Aircraft GetById(int id);
    }
}
