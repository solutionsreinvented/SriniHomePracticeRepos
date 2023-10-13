using System.Collections.Generic;

using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Models;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Design.ExcelInterop.Interfaces
{
    public interface ISectionDesignData
    {
        List<MemberForces> ForcesSummary { get; set; }

        MaterialTable MaterialTable { get; set; }

        MaterialGrade MaterialGrade { get; set; }

        DesignMethod DesignMethod { get; set; }

        Dictionary<RolledSectionKey, IAxialStrengthParameters> AxialStrengthParametersDictionary { get; set; }

        Dictionary<RolledSectionKey, IShearStrengthParameters> ShearStrengthParametersDictionary { get; set; }
    }
}
