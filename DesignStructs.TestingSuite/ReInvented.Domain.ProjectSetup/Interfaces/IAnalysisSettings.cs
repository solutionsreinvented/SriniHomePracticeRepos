using System.ComponentModel;

using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IAnalysisSettings : INotifyPropertyChanged
    {
        bool CanStartStaadApplication { get; set; }

        HiddenMode HiddenMode { get; set; }

        SilentMode SilentMode { get; set; }

        WaitAnalysisMode WaitMode { get; set; }
    }
}