using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ReIn.MvvmDialogs
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly bool _canExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute, bool canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private ICommand _showDialogCommand;
        readonly IDialogService _dialogService = new DialogService();

        public MainWindowViewModel()
        {

        }

        public ICommand ShowDialogCommand => _showDialogCommand ?? (_showDialogCommand = new DelegateCommand(OnShowDialog, true));

        private void OnShowDialog()
        {
            _dialogService.ShowDialog();
        }
    }
}
