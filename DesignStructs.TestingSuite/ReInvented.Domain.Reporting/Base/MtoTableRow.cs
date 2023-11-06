
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Reporting.Base
{
    public abstract class MtoTableRow : IMtoTableRow
    {
        #region Public Properties

        public double TotalWeight { get; set; }

        public MaterialGrade MaterialGrade { get; set; }

        #endregion
    }
}
