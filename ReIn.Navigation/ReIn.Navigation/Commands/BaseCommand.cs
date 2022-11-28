using System;
using System.Windows.Input;

namespace ReIn.Navigation.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public BaseCommand()
        {

        }

        public bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);

        protected void OnExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
