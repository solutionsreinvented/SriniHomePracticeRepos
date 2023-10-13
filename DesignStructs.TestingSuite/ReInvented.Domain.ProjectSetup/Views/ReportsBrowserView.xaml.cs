using System.Windows;
using System.Windows.Input;

namespace ReInvented.Domain.ProjectSetup.Views
{
    /// <summary>
    /// Interaction logic for ReportsBrowserView.xaml
    /// </summary>
    public partial class ReportsBrowserView : Window
    {
        public ReportsBrowserView()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
