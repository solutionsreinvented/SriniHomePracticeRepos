using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Factories;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

namespace ActivityTracker.Domain.Models
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
