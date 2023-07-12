using System;
using System.Windows;

using StockAnalyzer.Interfaces;

namespace StockAnalyzer.Services
{
    public class InstanceManager : IInstanceManager
    {
        public InstanceManager(Application current)
        {
            Current = current;
        }

        public Application Current { get; private set; }

        public void Start()
        {
            if (ShreddingService.ShreddingIsRequired)
            {
                ShreddingService.Shred(Current);
            }
            else
            {
                Current.Dispatcher.Invoke(() =>
                {
                    Current.MainWindow = new MainWindow();
                    Current.MainWindow.Show();
                });
            }
        }
    }
}
