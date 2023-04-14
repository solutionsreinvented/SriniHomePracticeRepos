using System;
using System.Windows.Input;

namespace ActivityTracker.UI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly bool _canExecute;
        private object onLogout;
        private bool v;

        public RelayCommand(Action execute, bool canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(object onLogout, bool v)
        {
            this.onLogout = onLogout;
            this.v = v;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
