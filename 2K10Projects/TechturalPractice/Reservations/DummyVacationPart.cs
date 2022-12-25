using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    public class DummyVacationPart : IVacationPart
    {
        public void Reserve()
        {
            throw new NotImplementedException();
        }

        public void Purchase()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
