using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReInvented.DialogManagement.Interfaces
{
    public interface IAsyncCommand<in T> : ICommand
    {
        IEnumerable<Task> RunningTasks { get; }

        bool CanExecute(T parameter);

        Task ExecuteAsync(T parameter);
    }
}
