using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReInvented.DialogManagement.Interfaces
{
    public interface IAsyncCommand : ICommand
    {
        IEnumerable<Task> RunningTasks { get; }

        bool CanExecute();

        Task ExecuteAsync();
    }
}
