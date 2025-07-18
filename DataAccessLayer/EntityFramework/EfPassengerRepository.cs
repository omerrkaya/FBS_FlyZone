﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EfPassengerRepository : IGenericDal<Passenger>, IPassengerDal
    {
        public void Delete(Passenger genent)
        {
            throw new NotImplementedException();
        }

        public List<Passenger> GetAll()
        {
            throw new NotImplementedException();
        }

        public Passenger GetById(int id)
        {
            using (var context = new Context())
            {
                // UserID'ye göre yolcu bilgilerini sorguluyoruz
                var passenger = context.Passengers.FirstOrDefault(p => p.UserID == id);

                return passenger;
            }

        }

        public Passenger GetPassengerByUserIdAndTcNo(string tcOrPassport)
        {
            using (var context = new Context())
            {
                return context.Passengers.FirstOrDefault(p =>  p.TcNo_PasaportNo == tcOrPassport);
            }
        }

        public void Insert(Passenger genent)
        {
            using (var c = new Context())
            {
                c.Passengers.Add(genent);
                c.SaveChanges();
            }
        }

        public void Update(Passenger genent)
        {
            throw new NotImplementedException();
        }
    }
}
