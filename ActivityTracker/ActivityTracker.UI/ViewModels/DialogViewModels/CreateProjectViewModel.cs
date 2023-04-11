using System;
using System.Windows.Input;

using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Stores;
using ActivityTracker.UI.Commands;

using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.ViewModels
{
    public class CreateProjectViewModel : PropertyStore, IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public CreateProjectViewModel()
        {
            ProjectDefinition = new ProjectDefinition();
            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), true);
            DiscardCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        public ProjectDefinition ProjectDefinition { get => Get<ProjectDefinition>(); set => Set(value); }

        public ICommand SaveCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DiscardCommand { get => Get<ICommand>(); private set => Set(value); }

    }
}
