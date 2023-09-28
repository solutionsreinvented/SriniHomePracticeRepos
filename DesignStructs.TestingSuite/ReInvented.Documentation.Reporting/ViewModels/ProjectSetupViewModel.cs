using System;
using System.Windows.Input;

using ReInvented.Documentation.Reporting.Models;
using ReInvented.Shared.Commands;

namespace ReInvented.Documentation.Reporting.ViewModels
{
    public class ProjectSetupViewModel : BaseViewModel
    {
        public ProjectSetupViewModel()
        {
            ProjectSettings = new ProjectSettings();
            VerificationCommand = new RelayCommand(OnVerification, true);
        }

        private void OnVerification()
        {

        }

        public ProjectSettings ProjectSettings { get => Get<ProjectSettings>(); private set => Set(value); }

        public ICommand VerificationCommand { get; set; }
    }
}
