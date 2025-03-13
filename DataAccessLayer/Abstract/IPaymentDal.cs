using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IPaymentDal
    {
        List<Payment> GetAll();
        void Add(Payment payment);
        void Update(Payment payment);
        void Delete(Payment payment);
        Payment GetById(int id);
    }
}
