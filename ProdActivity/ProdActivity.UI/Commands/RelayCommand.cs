using System;
using System.Windows.Input;

namespace ProdActivity.UI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Action<object> _executeWithParam;
        private readonly bool _canExecute;

        public RelayCommand(Action execute, bool canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> executeWithParam, bool canExecute)
        {
            _executeWithParam = executeWithParam;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            if (_executeWithParam != null)
                _executeWithParam(parameter);
            else
                _execute?.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
