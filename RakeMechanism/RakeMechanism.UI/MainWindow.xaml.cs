using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RakeMechanism.UI.ViewModels;

namespace RakeMechanism.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnAddSegment_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is CentralStructureViewModel centralVm)
        {
            var dialog = new AddSegmentDialog
            {
                Owner = this
            };

            if (dialog.ShowDialog() == true)
            {
                centralVm.AddSegment(dialog.ViewModel.NewSegment);
            }
        }
    }
}