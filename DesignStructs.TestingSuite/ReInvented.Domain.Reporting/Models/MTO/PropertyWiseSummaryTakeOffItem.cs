
using ReInvented.Domain.Reporting.Models.Base;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummaryTakeOffItem : TakeOffItem
    {
        public PropertyWiseSummaryTakeOffItem()
        {
        }

        public string Property { get; set; }

        public double Length { get; set; }

        public string Material { get; set; }
    }
}
