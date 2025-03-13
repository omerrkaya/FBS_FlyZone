using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;

namespace BusinessLayer.Concrete
{
    internal class GenericManager<A> : IGenericService<A> where A : class
    {
        
        public void Delete(A a)
        {
            throw new NotImplementedException();
        }

        public List<A> GetAll()
        {
            throw new NotImplementedException();
        }

        public A GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(A a)
        {
            throw new NotImplementedException();
        }

        public void Update(A a)
        {
            throw new NotImplementedException();
        }
    }
}
