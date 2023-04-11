using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Factories;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

namespace ActivityTracker.Domain.Models
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
