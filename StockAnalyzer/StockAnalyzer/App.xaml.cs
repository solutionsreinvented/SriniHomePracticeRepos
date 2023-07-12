using System.Windows;

using StockAnalyzer.Interfaces;
using StockAnalyzer.Services;

namespace StockAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IInstanceManager instanceManager = new InstanceManager(Current);
            instanceManager.Start();
        }
    }
}
