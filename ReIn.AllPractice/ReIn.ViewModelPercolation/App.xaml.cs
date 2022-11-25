using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ReIn.ViewModelPercolation.Stores;
using ReIn.ViewModelPercolation.ViewModels;

namespace ReIn.ViewModelPercolation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore _navigationStore = new NavigationStore();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow() { DataContext = new MainViewModel(_navigationStore) };
        }
    }
}
