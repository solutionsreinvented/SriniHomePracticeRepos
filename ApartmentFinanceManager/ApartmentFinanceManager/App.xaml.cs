using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ApartmentFinanceManager.Models;

namespace ApartmentFinanceManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();

            MainWindow.Show();

            ///var spentOn = new DateOnly(2022, 10, 3);

            ///Expense Expense = new Expense(Enums.ExpenseCategory.Maintenance, 2800m);///, spentOn);
        }



    }
}
