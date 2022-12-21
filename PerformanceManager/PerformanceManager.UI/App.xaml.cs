using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Services;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;
using PerformanceManager.UI.ViewModels;

namespace PerformanceManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void PrintServiceInfo(UtilizationCalculationService service)
        {
            Debug.Print($"Target year type: {service.AssessmentYear.GetType().Name}");
            Debug.Print($"Start date of the year: {service.AssessmentYear.StartDate}");
            Debug.Print($"End date of the year: {service.AssessmentYear.EndDate}");
            Debug.Print($"Number of working days: {service.AssessmentYear.CategorizedDays.WorkDays.Count()}");
            Debug.Print($"Number of holidays: {service.AssessmentYear.CategorizedDays.Holidays.Count()}");
            Debug.Print($"Number of weekends: {service.AssessmentYear.CategorizedDays.WeekEnds.Count()}");
        }

        private void UtilizationServiceTester()
        {
            IYear calendarYear = new CalendarYear(2023);
            IYear financialYear = new FinancialYear(2023);


            UtilizationCalculationService service = new UtilizationCalculationService(calendarYear);
            service = new UtilizationCalculationService(financialYear);



        }

        protected override void OnStartup(StartupEventArgs e)
        {
            UtilizationServiceTester();

            base.OnStartup(e);

            NavigationStore navigationStore = new();

            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            ///navigationStore.CurrentViewModel = new CreateActivityViewModel(navigationStore);

            ///navigationStore.CurrentViewModel = new DashboardViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
                {
                    CloseCommand = new RelayCommand(OnClose, true),
                    MinimizeCommand = new RelayCommand(OnMinimize, true),
                    MaximizeRestoreCommand = new RelayCommand(OnMaximizeRestore, true)
                }
            };

            MainWindow.Show();

        }



        private void OnMaximizeRestore()
        {
            Current.MainWindow.WindowState = Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnMinimize()
        {
            Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OnClose()
        {
            Current.MainWindow.Close();
        }
    }
}
