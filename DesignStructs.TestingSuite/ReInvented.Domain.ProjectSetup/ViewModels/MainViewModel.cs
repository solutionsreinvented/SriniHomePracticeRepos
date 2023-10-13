using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.ViewModels
{
    public class MainViewModel : ErrorsEnabledPropertyStore
    {
        public MainViewModel()
        {
            IProject project = new Project();
            CurrentViewModel = new ProjectSetupViewModel(project);
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }
    }
}
