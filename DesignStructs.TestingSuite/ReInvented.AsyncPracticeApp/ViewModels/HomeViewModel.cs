using System.Windows;
using System.Windows.Input;

using ReInvented.AsyncPracticeApp.Base;
using ReInvented.AsyncPracticeApp.Models;
using ReInvented.Shared.Commands;

namespace ReInvented.AsyncPracticeApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(Application application, IViewProvider viewProvider) : base(application, viewProvider)
        {
            SwitchToProgressViewCommand = new RelayCommand(OnSwitchToProgressView, true);
        }

        private void OnSwitchToProgressView()
        {
            Application.MainWindow = ViewProvider.GetProgressView();
            Application.MainWindow.DataContext = new ProgressViewModel(Application, ViewProvider);
            Application.MainWindow.Show();
        }

        public ICommand SwitchToProgressViewCommand { get; private set; }
    }
}
