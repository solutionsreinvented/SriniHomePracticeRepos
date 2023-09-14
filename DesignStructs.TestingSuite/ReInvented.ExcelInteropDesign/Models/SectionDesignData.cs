using System.Collections.Generic;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class SectionDesignData
    {
        public HashSet<MemberForces> MemberForcesCollection { get; set; }

        public double Fy { get; set; }

        public double Fu { get; set; }


    }
}
