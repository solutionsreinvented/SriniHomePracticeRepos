using System.Collections.Generic;

using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class SectionDesignData
    {
        public SectionDesignData()
        {
            DesignMethod = DesignMethod.LRFD;
        }

        public List<MemberForces> ForcesSummary { get; set; }

        public MaterialTable MaterialTable { get; set; }

        public MaterialGrade MaterialGrade { get; set; }

        public WebTransverseStiffeners Stiffeners { get; set; }

        public AxialStrengthParameters AxialStrengthParameters { get; set; }

        public DesignMethod DesignMethod { get; set; }
    }
}
