using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public class MemberForces
    {
        public int LoadCaseId { get; set; }

        public string LoadCaseTitle { get; set; }

        public int MemberId { get; set; }

        public MemberEnd End { get; set; }

        public double Fx { get; set; }

        public double Fy { get; set; }

        public double Fz { get; set; }

        public double Mx { get; set; }

        public double My { get; set; }

        public double Mz { get; set; }

    }
}
