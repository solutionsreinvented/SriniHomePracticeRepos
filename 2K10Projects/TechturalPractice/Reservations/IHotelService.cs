using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public interface IHotelService
    {
        IVacationPart MakeBooking(HotelInfo hotel, DateTime checkin, DateTime checkout);
    }
}
