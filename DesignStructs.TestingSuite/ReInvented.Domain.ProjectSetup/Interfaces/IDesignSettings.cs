using System.ComponentModel;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IDesignSettings : INotifyPropertyChanged
    {
        bool CheckNonPreferredSectionsAlso { get; set; }

        int DesignIterationsLimit { get; set; }

        bool LimitDesignIterations { get; set; }

        bool PerformAutoDesign { get; set; }
    }
}