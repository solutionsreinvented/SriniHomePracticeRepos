using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Factories;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Stores;

namespace ProdActivity.Domain.Models
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
