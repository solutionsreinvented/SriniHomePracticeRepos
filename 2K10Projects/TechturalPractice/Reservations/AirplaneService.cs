using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Reservations
{
    public class AirplaneService : IAirplaneService
    {
        public IVacationPart SelectFlight(string companyName, string origin, string destination, DateTime travelDate)
        {
            Console.WriteLine("Waiting for air ticketing service to respond...");
            Thread.Sleep(500);

            Console.WriteLine("Booking flight {0} - {1} on {2:dd-MMM-yyyy}\n", origin, destination, travelDate);

            return new DummyVacationPart();
        }
    }
}
