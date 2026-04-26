using System.Windows;
using RakeMechanism.UI.ViewModels;

namespace RakeMechanism.UI
{
    public partial class AddSegmentDialog : Window
    {
        public AddSegmentViewModel ViewModel { get; }

        public AddSegmentDialog()
        {
            InitializeComponent();
            ViewModel = new AddSegmentViewModel();
            DataContext = ViewModel;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
