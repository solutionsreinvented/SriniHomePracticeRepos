using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public class HotelInfo
    {
        public string Town { get; set; }

        public string HotelName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} in {1}", HotelName, Town);
        }
    }
}
