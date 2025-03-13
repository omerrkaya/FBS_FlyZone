using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<A> where A : class
    {
        void Insert(A a);
        void Delete(A a);
        void Update(A a);
        List<A> GetAll();
        A GetById(int id);

    }
}
