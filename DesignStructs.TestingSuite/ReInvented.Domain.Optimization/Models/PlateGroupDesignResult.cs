using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.Domain.Optimization.Models
{
    public sealed class PlateGroupDesignResult
    {
        #region Default Constructor

        public PlateGroupDesignResult()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PlateGroupDesignResult(string groupName, PlateGroupStressSummary stressSummary, double currentThickness)
        {
            GroupName = groupName;
            StressSummary = stressSummary;
            CurrentThickness = currentThickness;
            DesignThickness = currentThickness;
        }

        #endregion

        #region Public Properties

        public PlateGroupStressSummary StressSummary { get; set; }
        public string GroupName { get; set; }
        public double DesignThickness { get; set; }
        public double CurrentThickness { get; set; }
        public double CorrosionAllowance { get; set; }
        public double MinimumThickness { get; set; }

        #endregion
    }
}
