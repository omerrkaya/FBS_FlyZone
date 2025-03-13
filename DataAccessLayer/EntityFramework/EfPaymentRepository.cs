using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfPaymentRepository : IGenericDal<Payment> , IPaymentDal
    {
        public void Delete(Payment genent)
        {
            throw new NotImplementedException();
        }
        public List<Payment> GetAll()
        {
            throw new NotImplementedException();
        }
        public Payment GetById(int id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Payment genent)
        {
            throw new NotImplementedException();
        }
        public void Update(Payment genent)
        {
            throw new NotImplementedException();
        }
    }
    {
    }
}
