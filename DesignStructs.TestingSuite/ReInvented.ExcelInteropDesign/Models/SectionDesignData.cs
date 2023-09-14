using System.Collections.Generic;

using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class SectionDesignData
    {
        public HashSet<MemberForces> MemberForcesCollection { get; set; }

        public MaterialGrade MaterialGrade { get; set; }


    }
}
