using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-H3SQ8GC;" +
                "Database=FBS_FlyzoneDb;" +
                //"User Id = ......;" +
                //"Password = ......;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;");

            // Umut için inmemory database, yukarısı silinir ve aşağısı açılır, Lütfen normal normal şartlarda dokunmayın.
               // optionsBuilder.UseInMemoryDatabase("FBS_FlyzoneDb");

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }



    }
}
