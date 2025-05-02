using EntityLayer.Concrete;

namespace FBS_FlyZone.Models
{
    public class PaymentDetailsViewModel
    {
        public Payment Payment { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
