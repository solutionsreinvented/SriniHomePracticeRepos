using ReInvented.Domain.Reporting.Models.Base;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummaryItem : SummaryItem
    {
        #region Default Constructor

        public PropertyWiseSummaryItem()
        {

        }

        #endregion

        #region Public Properties

        public string ProfileOrThickness { get; set; }

        public double LengthOrArea { get; set; }

        public string MaterialGrade { get; set; }

        #endregion
    }
}
