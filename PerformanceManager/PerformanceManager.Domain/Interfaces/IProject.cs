using PerformanceManager.Domain.Enums;

using System.Collections.ObjectModel;

namespace PerformanceManager.Domain.Interfaces
{
    public interface IProject
    {
        ProjectType Type { get; }

        string Code { get; }

        string Name { get; }

        ObservableCollection<IActivity> Activities { get; set; }
    }
}