using System.Windows;

using ReInvented.FluentValidationExercise.Models;
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
            base.OnStartup(e);
            Input input = new Input();

            MainWindow = new MainWindow() { DataContext = new MainViewModel() };

            MainWindow.Show();


        }
    }
}
