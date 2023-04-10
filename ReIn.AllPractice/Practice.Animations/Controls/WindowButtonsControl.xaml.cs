using System.Windows;
using System.Windows.Controls;

namespace Practice.DesignParameters.Controls
{
    /// <summary>
    /// Interaction logic for WindowButtonsControl.xaml
    /// </summary>
    public partial class WindowButtonsControl : UserControl
    {
        private Window _hostWindow;

        public WindowButtonsControl()
        {
            InitializeComponent();
        }


        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SetHostWindow();
            _hostWindow.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeOrRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            SetHostWindow();
            _hostWindow.WindowState = _hostWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            SetHostWindow();
            _hostWindow.Close();
        }

        private void SetHostWindow()
        {
            _hostWindow = _hostWindow ?? Window.GetWindow(this);
        }
    }
}
