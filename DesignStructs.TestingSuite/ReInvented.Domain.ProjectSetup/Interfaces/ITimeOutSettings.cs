using System.ComponentModel;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface ITimeOutSettings : INotifyPropertyChanged
    {
        double AnalysisTimeout { get; set; }

        double AutoDesignTimeout { get; set; }

        double FileOpeningTimeout { get; set; }

        double ModelAvailabilityCheckInterval { get; set; }
    }
}