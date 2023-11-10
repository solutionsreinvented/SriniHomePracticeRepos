using System.Collections.Generic;
using System.Linq;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummary
    {
        #region Default Constructor

        public PropertyWiseSummary()
        {

        }

        #endregion

        #region Public Properties

        public HashSet<SectionsSummaryItem> SectionsItems { get; set; }

        public HashSet<PlatesSummaryItem> PlatesItems { get; set; }

        public double SectionsTotalWeight => SectionsItems == null ? 0.0 : SectionsItems.Sum(i => i.Weight);

        public double PlatesTotalWeight => PlatesItems == null ? 0.0 : PlatesItems.Sum(i => i.Weight);

        #endregion
    }
}
