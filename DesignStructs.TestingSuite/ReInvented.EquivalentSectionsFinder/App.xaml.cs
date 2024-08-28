using System.Windows;

using Continuum.EquivalentSectionsFinder.Views;

namespace Continuum.EquivalentSectionsFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new EquivalentSectionsView();
            MainWindow.Show();
        }
    }
}
