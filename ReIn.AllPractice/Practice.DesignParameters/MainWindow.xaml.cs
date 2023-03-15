using System.Windows;

namespace Practice.DesignParameters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DesignParametersProcessor.AssignParameters();
        }
    }
}
