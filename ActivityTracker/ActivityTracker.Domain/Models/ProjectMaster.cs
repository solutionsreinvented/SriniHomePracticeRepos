using Newtonsoft.Json;

using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Stores;

using System.Collections.ObjectModel;

namespace ProdActivity.Domain.Models
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
