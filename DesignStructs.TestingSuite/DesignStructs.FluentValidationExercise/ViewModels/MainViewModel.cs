using System.Windows.Input;

using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace DesignStructs.FluentValidationExercise.ViewModels
{
    public class MainViewModel : ValidatablePropertyStore
    {

        public MainViewModel()
        {
            TestCommand = new RelayCommand(OnTest, true);
        }

        private void OnTest()
        {
            ///throw new NotImplementedException();
        }

        public ICommand TestCommand { get; set; }
    }
}
