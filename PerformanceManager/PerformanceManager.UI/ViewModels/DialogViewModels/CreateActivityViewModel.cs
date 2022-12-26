using System;
using System.Windows.Input;

using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Stores;
using PerformanceManager.UI.Commands;

using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public class CreateActivityViewModel : PropertyStore, IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public CreateActivityViewModel()
        {
            ActivityDefinition = new ActivityDefinition();

            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), true);
            DiscardCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        public ActivityDefinition ActivityDefinition { get => Get<ActivityDefinition>(); private set => Set(value); }

        public bool MyProperty { get; set; }

        public ICommand SaveCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DiscardCommand { get => Get<ICommand>(); private set => Set(value); }
    }
}
