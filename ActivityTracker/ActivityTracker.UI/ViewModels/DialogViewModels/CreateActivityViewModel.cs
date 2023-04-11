using System;
using System.Windows.Input;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Stores;
using ActivityTracker.UI.Commands;

using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.ViewModels
{
    public class CreateActivityViewModel : PropertyStore, IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public CreateActivityViewModel(IProject selectedProject)
        {
            ActivityDefinition = new ActivityDefinition(selectedProject);

            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), true);
            DiscardCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        public ActivityDefinition ActivityDefinition { get => Get<ActivityDefinition>(); private set => Set(value); }

        public bool MyProperty { get; set; }

        public ICommand SaveCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DiscardCommand { get => Get<ICommand>(); private set => Set(value); }
    }
}
