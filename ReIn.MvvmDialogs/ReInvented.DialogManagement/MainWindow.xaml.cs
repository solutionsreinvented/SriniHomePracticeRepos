using ReInvented.DialogManagement.Interfaces;
using System.Windows;

namespace ReInvented.DialogManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDialog
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
