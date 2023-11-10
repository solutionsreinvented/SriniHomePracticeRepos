using ReInvented.Domain.Reporting.Models.Base;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PlatesSummaryItem : SummaryItem
    {
        #region Default Constructor

        public PlatesSummaryItem()
        {

        }

        #endregion

        #region Public Properties

        public string Thickness { get; set; }

        public double Area { get; set; }

        public string MaterialGrade { get; set; }

        #endregion
    }
}
