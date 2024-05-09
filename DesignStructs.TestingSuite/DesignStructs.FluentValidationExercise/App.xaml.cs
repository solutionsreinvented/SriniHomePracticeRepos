using System.Windows;

using DesignStructs.FluentValidationExercise.ViewModels;

namespace ReInvented.FluentValidationExercise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainViewModel viewModel = new MainViewModel();

            base.OnStartup(e);
            MainWindow = new MainWindow() { DataContext = viewModel };

            MainWindow.Show();


        }
    }
}
