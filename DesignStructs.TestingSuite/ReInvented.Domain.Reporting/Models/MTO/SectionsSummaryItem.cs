using ReInvented.Domain.Reporting.Models.Base;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class SectionsSummaryItem : SummaryItem
    {
        #region Default Constructor

        public SectionsSummaryItem()
        {

        }

        #endregion

        #region Public Properties

        public string Section { get; set; }

        public double Length { get; set; }

        public string MaterialGrade { get; set; }

        #endregion
    }
}
