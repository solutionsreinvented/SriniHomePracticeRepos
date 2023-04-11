using Newtonsoft.Json;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

using System.Collections.ObjectModel;

namespace ActivityTracker.Domain.Models
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
