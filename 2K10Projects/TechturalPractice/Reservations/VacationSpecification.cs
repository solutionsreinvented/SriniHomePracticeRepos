using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reservations
{
    class VacationSpecification
    {
        private IList<IVacationPart> parts;

        public VacationSpecification(IList<IVacationPart> parts)
        {
            this.parts = parts;
        }
    }
}
