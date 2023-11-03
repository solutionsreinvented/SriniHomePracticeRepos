using ReInvented.Domain.Reporting.Models.Base;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummaryItem : SummaryItem
    {
        public PropertyWiseSummaryItem()
        {

        }

        public string ProfileOrThickness { get; set; }

        public double LengthOrArea { get; set; }

        public string MaterialGrade { get; set; }
    }
}
