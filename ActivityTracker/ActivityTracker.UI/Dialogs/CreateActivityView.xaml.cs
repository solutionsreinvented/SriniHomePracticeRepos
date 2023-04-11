using System.Windows;

using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateActivityView.xaml
    /// </summary>
    public partial class CreateActivityView : Window, IDialog
    {
        public CreateActivityView()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
