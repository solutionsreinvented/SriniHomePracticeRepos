using System.ComponentModel;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        IProjectInfo ProjectInfo { get; }

        ITimeOutSettings TimeOutSettings { get; }

        IAnalysisSettings AnalysisSettings { get; }

        IDesignSettings DesignSettings { get; }

        IReportSettings ReportSettings { get; }

        IPerformanceSettings PerformanceSettings { get; }
    }
}