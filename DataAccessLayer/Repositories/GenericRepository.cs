using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<Gen_Ent> : IGenericDal<Gen_Ent> where Gen_Ent : class
    {
        public void Delete(Gen_Ent genent)
        {
            using var c = new Context();
            c.Remove(genent);
            c.SaveChanges();
        }

        public List<Gen_Ent> GetAll()
        {
            using var c = new Context();
            return c.Set<Gen_Ent>().ToList();

        }

        public Gen_Ent GetById(int id)
        {
            using var c = new Context();
            return c.Set<Gen_Ent>().Find(keyValues: id) ?? throw new InvalidOperationException("Entity not found");
        }

        public void Insert(Gen_Ent genent)
        {
           using var c = new Context();
            c.Add(genent);
            c.SaveChanges();

        }

        public void Update(Gen_Ent genent)
        {
            using var c = new Context();
            c.Update(genent);
            c.SaveChanges();
        }
    }
}
