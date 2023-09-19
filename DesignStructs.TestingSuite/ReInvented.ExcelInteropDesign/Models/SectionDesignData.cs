using System.Collections.Generic;

using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class SectionDesignData : ISectionDesignData
    {
        public SectionDesignData()
        {
            DesignMethod = DesignMethod.LRFD;
        }

        public List<MemberForces> ForcesSummary { get; set; }

        public MaterialTable MaterialTable { get; set; }

        public MaterialGrade MaterialGrade { get; set; }

        public DesignMethod DesignMethod { get; set; }

        public IAxialStrengthParameters AxialStrengthParameters { get; set; }

        public IShearStrengthParameters ShearStrengthParameters { get; set; }
    }
}
