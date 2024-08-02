using System.Windows;

using ReInvented.Shared.Extensions;

namespace ReInvented.MathJaxer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var result = MathJaxHelpers.LongRandomExample();

            base.OnStartup(e);
        }
    }
}
