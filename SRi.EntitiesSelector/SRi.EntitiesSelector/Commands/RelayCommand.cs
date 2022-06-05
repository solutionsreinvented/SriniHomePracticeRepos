using System;
using System.Windows.Input;

namespace SRi.EntitiesSelector.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _action;
        private readonly bool _canExecute;

        public RelayCommand(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter) => _action();
    }
}
