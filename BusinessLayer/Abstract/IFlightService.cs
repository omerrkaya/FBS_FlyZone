using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{

    public interface IFlightService
    {
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(Flight flight);
        Flight GetFlightById(int id);
        List<Flight> GetListAll();
    }

}
