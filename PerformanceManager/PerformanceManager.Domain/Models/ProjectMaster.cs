using Newtonsoft.Json;

using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

using System.Collections.ObjectModel;

namespace PerformanceManager.Domain.Models
{
    public class ProjectMaster : PropertyStore
    {
        public ProjectMaster()
        {
            Projects = new ObservableCollection<IProject>();
        }
        
        public ObservableCollection<IProject> Projects { get => Get<ObservableCollection<IProject>>(); set => Set(value); }
    }
}
