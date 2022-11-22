using System.Windows;

using ReInvented.UI.Themes.Enums;
using ReInvented.UI.Themes.Models;

namespace ReIn.TreeViewStyling
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Theme.LoadThemeType(ThemeType.Dark);
        }
    }
}
