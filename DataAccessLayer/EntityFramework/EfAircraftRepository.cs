using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfAircraftRepository : IGenericDal<Aircraft>, IAircraftDal
    {
        public void Delete(Aircraft genent)
        {
            throw new NotImplementedException();
        }

        public List<Aircraft> GetAll()
        {
            throw new NotImplementedException();
        }

        public Aircraft GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Aircraft genent)
        {
            throw new NotImplementedException();
        }

        public void Update(Aircraft genent)
        {
            throw new NotImplementedException();
        }
    }
}
