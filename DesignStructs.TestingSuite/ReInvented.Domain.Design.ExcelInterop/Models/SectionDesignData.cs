using System.Collections.Generic;

using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Design.ExcelInterop.Models
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

        public Dictionary<RolledSectionKey, IAxialStrengthParameters> AxialStrengthParametersDictionary { get; set; }

        public Dictionary<RolledSectionKey, IShearStrengthParameters> ShearStrengthParametersDictionary { get; set; }
    }
}
