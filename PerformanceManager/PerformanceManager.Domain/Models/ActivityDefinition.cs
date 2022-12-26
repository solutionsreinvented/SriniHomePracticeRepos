using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Factories;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Models
{
    public class ActivityDefinition : PropertyStore
    {
        private readonly IProject _selectedProject;

        public ActivityDefinition(IProject selectedProject)
        {
            _selectedProject = selectedProject;

            Discipline = Discipline.Design;
        }

        public Discipline Discipline { get => Get<Discipline>(); set { Set(value); UpdateActivity(); } }

        private void UpdateActivity()
        {
            Activity = ActivityFactory.Create(Discipline, _selectedProject);
        }

        public IActivity Activity { get => Get<IActivity>(); set => Set(value); }
    }
}
