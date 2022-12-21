using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public interface IVacationPart
    {
        void Reserve();
        void Purchase();
        void Cancel();
    }
}
