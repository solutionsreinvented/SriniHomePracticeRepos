using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using TechturalPractice.Models;

namespace TechturalPractice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            Account account = new Account();
            account.Deposit(10);
            Console.WriteLine("Current balance: {0}", account.Balance);
            account.Deposit(25);
            Console.WriteLine("Current balance: {0}", account.Balance);
            account.Deposit(15);
            Console.WriteLine("Current balance: {0}", account.Balance);


            base.OnStartup(e);
        }


    }
}
