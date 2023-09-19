using System.Collections.Generic;

using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Interfaces
{
    public interface ISectionDesignData
    {
        List<MemberForces> ForcesSummary { get; set; }

        MaterialTable MaterialTable { get; set; }

        MaterialGrade MaterialGrade { get; set; }

        DesignMethod DesignMethod { get; set; }

        IAxialStrengthParameters AxialStrengthParameters { get; set; }

        IShearStrengthParameters ShearStrengthParameters { get; set; }
    }
}
