using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.ViewModels
{
    public class BaseViewModel : ErrorsEnabledPropertyStore
    {
        public BaseViewModel(IProject project)
        {
            Project = project;
        }

        public IProject Project { get => Get<IProject>(); private set => Set(value); }
    }
}
