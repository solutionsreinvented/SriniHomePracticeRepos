using System;

using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Stores;

using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public class CreateProjectViewModel : PropertyStore, IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public CreateProjectViewModel()
        {
            ProjectDefinition = new ProjectDefinition();
        }

        public ProjectDefinition ProjectDefinition { get => Get<ProjectDefinition>(); set => Set(value); }
    }
}
