using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public class VacationPartFactory : IVacationPartFactory
    {
        IHotelSelector _hotelSelector;
        IHotelService _hotelService;
        IAirplaneService _airplaneService;

        public VacationPartFactory(IHotelService hotelService, IHotelSelector hotelSelector, IAirplaneService airplaneService)
        {
            _hotelService = hotelService;
            _hotelSelector = hotelSelector;
            _airplaneService = airplaneService;
        }


        public IVacationPart CreateHotelReservation(string town, string hotelName, DateTime arrivalDate, DateTime leaveDate)
        {
            HotelInfo hotel = _hotelSelector.SelectHotel(town, hotelName);

            return _hotelService.MakeBooking(hotel, arrivalDate, leaveDate);
        }

        public IVacationPart CreateFlight(string companyName, string source, string destination, DateTime date)
        {
            return _airplaneService.SelectFlight(companyName, source, destination, date);
        }

        public IVacationPart CreateFerryBooking(string lineName, bool fromMainland, DateTime date)
        {
            throw new NotImplementedException();
        }

        public IVacationPart CreateDailyTrip(string tripName, DateTime date)
        {
            throw new NotImplementedException();
        }

        public IVacationPart CreateMassage(string salon, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
