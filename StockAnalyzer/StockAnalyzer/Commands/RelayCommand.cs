using System;
using System.Windows.Input;

namespace StockAnalyzer.Commands
{
    public class RelayCommand : ICommand
    {

        private readonly Action _onExecute;

        private readonly bool _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action onExecute, bool canExecute)
        {
            _onExecute = onExecute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _onExecute();
        }
    }
}
