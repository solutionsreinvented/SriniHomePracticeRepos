using System;
using System.Windows.Input;

namespace WpfMvvmNavigation.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _execute;

        private readonly bool _canExecute;

        public RelayCommand(Action execute, bool canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
