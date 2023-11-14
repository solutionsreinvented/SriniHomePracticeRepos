using System.Collections.Generic;

using ReInvented.Domain.Reporting.Interfaces;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class FoundationLoadData : IReportContent
    {
        #region Default Constructor

        public FoundationLoadData()
        {

        }

        #endregion

        #region Public Properties

        public Dictionary<int, string> LoadCases { get; set; }

        public HashSet<PCDLoads> PCDLoadsCollection { get; set; }

        #endregion
    }
}
