using System;
using System.Windows;
using System.Windows.Threading;

namespace ProdActivity.UI.Views
{
    public partial class DesktopNotifierWindow : Window
    {
        private DispatcherTimer _closeTimer;

        public DesktopNotifierWindow()
        {
            InitializeComponent();
            
            // Position top-right
            var workArea = SystemParameters.WorkArea;
            this.Left = workArea.Right - this.Width - 10;
            this.Top = workArea.Top + 10;

            // Auto-close after 30 seconds
            _closeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            _closeTimer.Tick += (s, e) => Close();
            _closeTimer.Start();
        }

        private void OnMarkCompletedClick(object sender, RoutedEventArgs e)
        {
            // Save completion for today
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\ReInvented\ProdActivity");
                key?.SetValue("LastNotificationCompletedDate", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            }
            catch { }
            
            Close();
        }

        private void OnDismissClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _closeTimer?.Stop();
            base.OnClosed(e);
        }
    }
}
