using ReInvented.DialogManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReInvented.DialogManagement.Commands
{
    public abstract class AsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly ObservableCollection<Task> _runningTasks;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #region Default Constructor

        protected AsyncCommand()
        {
            _runningTasks = new ObservableCollection<Task>();
            _runningTasks.CollectionChanged += OnRunningTasksChanged;
        }

        #endregion

        public IEnumerable<Task> RunningTasks => _runningTasks;

        #region Explicit ICommand Implementation

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            Task runningTask = ExecuteAsync((T)parameter);

            _runningTasks.Add(runningTask);

            try
            {
                await runningTask;
            }
            finally
            {
                _runningTasks.Remove(runningTask);
            }

            await ExecuteAsync();
        }

        #endregion

        #region IAsyncCommand Methods

        public abstract bool CanExecute(T parameter);

        public abstract Task ExecuteAsync(T parameter);

        #endregion

        #region Private Helpers

        private void OnRunningTasksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

    }
}
