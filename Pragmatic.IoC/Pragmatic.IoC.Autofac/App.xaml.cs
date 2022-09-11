using System.Windows;
using Autofac;
using Pragmatic.IoC.Autofac.Models;
using Pragmatic.IoC.Autofac.ViewModels;

namespace Pragmatic.IoC.Autofac
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MainModel>();

            builder.Build();
        }
    }
}
