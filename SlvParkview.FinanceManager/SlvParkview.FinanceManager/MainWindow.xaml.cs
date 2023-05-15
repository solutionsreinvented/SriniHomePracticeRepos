using System.Windows;

namespace SlvParkview.FinanceManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(e.RightButton == System.Windows.Input.MouseButtonState.Pressed))
            {
                DragMove();
            }

        }
    }
}
