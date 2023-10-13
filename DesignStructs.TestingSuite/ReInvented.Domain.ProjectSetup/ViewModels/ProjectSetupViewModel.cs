using System.Windows.Input;

using ReInvented.DataAccess.Services;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Commands;

namespace ReInvented.Domain.ProjectSetup.ViewModels
{
    public class ProjectSetupViewModel : BaseViewModel
    {
        public ProjectSetupViewModel(IProject project) : base(project)
        {
            VerificationCommand = new RelayCommand(OnVerification, true);
            SelectProjectDirectoryCommand = new RelayCommand(OnSelectProjectDirectory, true);
        }

        public ICommand VerificationCommand { get; set; }

        public ICommand SelectProjectDirectoryCommand { get; set; }

        private void OnSelectProjectDirectory()
        {
            Project.Settings.ProjectInfo.ProjectDirectory = FileServiceProvider.GetDirectoryPathUsingFolderBrowserDialog();
        }

        private void OnVerification()
        {

        }
    }
}
