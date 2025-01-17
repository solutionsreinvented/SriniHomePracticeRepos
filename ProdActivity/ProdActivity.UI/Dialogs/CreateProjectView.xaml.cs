using System.Windows;
using System.Windows.Input;

using ReInvented.Shared.Interfaces;

namespace ProdActivity.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateProjectView.xaml
    /// </summary>
    public partial class CreateProjectView : Window, IDialog
    {
        public CreateProjectView()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
