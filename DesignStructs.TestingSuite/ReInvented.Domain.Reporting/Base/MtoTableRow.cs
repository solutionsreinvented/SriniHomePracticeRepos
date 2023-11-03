
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Reporting.Base
{
    public abstract class MtoTableRow : IMtoTableRow
    {
        public double TotalWeight { get; set; }

        public MaterialGrade MaterialGrade { get; set; }
    }
}
