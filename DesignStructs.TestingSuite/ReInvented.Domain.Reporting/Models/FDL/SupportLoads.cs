using System.Collections.Generic;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class SupportLoads
    {
        public SupportLoads() { }

        public Node Support { get; set; }

        public HashSet<LoadCaseForces> Loads { get; set; }
    }
}
