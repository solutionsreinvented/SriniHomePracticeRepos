using System.Collections.Generic;
using System.ComponentModel;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IPerformanceSettings : INotifyPropertyChanged
    {
        int NumberOfThreads { get; set; }

        IEnumerable<int> PossibleNumberOfThreads { get; }

        bool UseMultipleThreads { get; set; }
    }
}