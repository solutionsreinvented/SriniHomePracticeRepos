using System;
using System.Windows.Input;
using ReInvented.Documentation.Reporting.Interfaces;
using ReInvented.Documentation.Reporting.Models;
using ReInvented.Shared.Commands;

namespace ReInvented.Documentation.Reporting.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {
        public ProjectViewModel()
        {
            Project = new Project();
            VerificationCommand = new RelayCommand(OnVerification, true);
        }

        private void OnVerification()
        {

        }

        public IProject Project { get => Get<IProject>(); private set => Set(value); }

        public ICommand VerificationCommand { get; set; }
    }
}
