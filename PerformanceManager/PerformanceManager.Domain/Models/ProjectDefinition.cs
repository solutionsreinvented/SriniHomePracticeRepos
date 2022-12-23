using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Factories;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Models
{
    public class ProjectDefinition : PropertyStore
    {
        public ProjectDefinition()
        {
            ProjectType = ProjectType.Order;
        }

        public ProjectType ProjectType { get => Get<ProjectType>(); set { Set(value); UpdateProject(); } }

        private void UpdateProject()
        {
            Project = ProjectFactory.Create(ProjectType);
        }

        public IProject Project { get => Get<IProject>(); set => Set(value); }
    }
}
