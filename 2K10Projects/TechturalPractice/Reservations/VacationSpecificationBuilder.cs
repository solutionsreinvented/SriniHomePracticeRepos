using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public class VacationSpecificationBuilder
    {
        private IVacationPartFactory _partFactory;

        private IList<IVacationPart> _parts = new List<IVacationPart>();

        private DateTime _arrivalDate;
        private int _totalNights;

        private string _livingTown;
        private string _destinationTown;

        public VacationSpecificationBuilder(IVacationPartFactory partFactory,
                                            DateTime arrivalDate, int totalNights, 
                                            string livingTown, string destinationTown)
        {
            _partFactory = partFactory;

            _arrivalDate = arrivalDate;
            _totalNights = totalNights;

            _livingTown = livingTown;
            _destinationTown = destinationTown;
        }


        public void SelectHotel(string hotelName)
        {
            IVacationPart part = _partFactory.CreateHotelReservation(_destinationTown, hotelName, _arrivalDate, 
                                                                             _arrivalDate.AddDays(_totalNights));

            _parts.Add(part);
        }

        public void SelectAirplane(string companyName)
        {
            _parts.Add(CreateFlightToDestination(companyName));
            _parts.Add(CreateFlightBack(companyName));
        }

        private IVacationPart CreateFlightBack(string companyName)
        {
            return _partFactory.CreateFlight(companyName, _destinationTown, _livingTown, _arrivalDate.AddDays(_totalNights));
        }

        private IVacationPart CreateFlightToDestination(string companyName)
        {
            return _partFactory.CreateFlight(companyName, _livingTown, _destinationTown, _arrivalDate);
        }

        public VacationSpecification BuildSpecification()
        {
            foreach (IVacationPart part in _parts)
                part.Purchase();
            return new VacationSpecification(_parts);
        }
    }
}
