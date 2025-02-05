using System;
using System.Windows.Input;

using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Models;
using ProdActivity.Domain.Stores;
using ProdActivity.UI.Commands;

using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace ProdActivity.UI.ViewModels
{
    public class CreateActivityViewModel : PropertyStore, IDialogRequestClose
    {
        #region Events

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Parameterized Constructor

        public CreateActivityViewModel(IProject selectedProject)
        {
            ActivityDefinition = new ActivityDefinition(selectedProject);

            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), true);
            DiscardCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        #endregion

        #region Public Properties

        public ActivityDefinition ActivityDefinition { get => Get<ActivityDefinition>(); private set => Set(value); }

        public bool MyProperty { get; set; }

        #endregion

        #region Commands

        public ICommand SaveCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DiscardCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion
    }
}
