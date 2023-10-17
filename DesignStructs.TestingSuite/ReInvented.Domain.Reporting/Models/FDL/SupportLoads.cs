using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class SupportLoads
    {
        public SupportLoads() { }

        public int NodeNumber { get; set; }

        public HashSet<LoadCaseForces> Loads { get; set; }
    }
}
