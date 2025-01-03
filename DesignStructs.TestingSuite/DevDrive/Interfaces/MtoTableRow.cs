using ReInvented.Sections.Domain.Models;

namespace DevDrive.Interfaces
{
    public interface ISectionProperties
    {

    }
    public interface ISectionCommonProperties : ISectionProperties
    {
        double Ax { get; set; }
        double Ix { get; set; }
        double Iy { get; set; }
        double Iz { get; set; }
    }
    public interface IMtoTableRow
    {
        int PropertyId { get; }

        MaterialGrade MaterialGrade { get; set; }

        double TotalWeight { get; set; }
    }
}
