using System;
using System.IO;
using System.Threading;
using System.Windows;

using Microsoft.Win32;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

namespace ReInvented.SPro2023Wrapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StaadModel model = new StaadModel();

            string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\D11.3H03.00S09.00OC1.050SC1.128IMP0.439CON0.1665MOT0054.std";

        }

        private static void EnsureCreate(OpenSTAAD instance, string fullFilePath)
        {

        }
    }
}
