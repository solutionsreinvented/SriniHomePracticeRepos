using System.ComponentModel;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IReportSettings : INotifyPropertyChanged
    {
        bool GenerateContentLoadsCalculationReport { get; set; }

        bool GenerateMTO { get; set; }

        bool GenerateStructureSeismicCalculationReport { get; set; }

        bool IncludeMTONotes { get; set; }
    }
}