using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<Gen_Ent> where Gen_Ent : class
    {
        void Insert(Gen_Ent genent);
        void Delete(Gen_Ent genent);
        void Update(Gen_Ent genent);
        List<Gen_Ent> GetAll();
        Gen_Ent GetById(int id);

    }
}
