using ReInvented.DialogManagement.Commands;
using ReInvented.DialogManagement.EventsData;
using ReInvented.DialogManagement.Interfaces;
using System;
using System.Windows.Input;

namespace ReInvented.DialogManagement.ViewModels
{
    public class SectionSelectionViewModel : IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public SectionSelectionViewModel()
        {
            OkCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), true);
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        public string TestText { get; set; } = "Default Text";

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }
    }
}
