
using System.Collections.Generic;

using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Optimization.Models
{
    public class PlateOptimizationCriteria
    {
        #region Default Constructor

        public PlateOptimizationCriteria()
        {

        }

        #endregion

        #region Public Properties

        public int ThreadCount { get; set; } = 1;
        public double MinimumThickness { get; set; } = 0.0;
        public double CorrosionAllowance { get; set; } = 0.0;
        public double AllowedPercentPlatesExceedance { get; set; } = 10.0;
        public double PartialFactor { get; set; } = 0.90;
        public MaterialGrade MaterialGrade { get; set; }
        public HashSet<string> ExcludedGroupNames { get; set; } = new HashSet<string>() { "TANK", "COMP", "CENTRE", "LAUNDER" };

        public double LimitingStress => MaterialGrade == null ? 0.0 : PartialFactor * MaterialGrade.Fy;

        #endregion
    }
}
