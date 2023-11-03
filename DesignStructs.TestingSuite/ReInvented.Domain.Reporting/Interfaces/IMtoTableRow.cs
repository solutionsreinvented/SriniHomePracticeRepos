using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IMtoTableRow
    {
        MaterialGrade MaterialGrade { get; set; }

        double TotalWeight { get; set; }
    }

}
