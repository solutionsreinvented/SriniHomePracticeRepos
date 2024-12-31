using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DevDrive
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

        private void OpenColorPicker(object sender, RoutedEventArgs e)
        {
            ColorPopup.IsOpen = true;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null) return;

            // Get the clicked position
            Point position = e.GetPosition(canvas);

            // Create a RenderTargetBitmap of the canvas
            var renderTarget = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(canvas);

            // Get the color at the clicked position
            var pixel = new byte[4];
            renderTarget.CopyPixels(new Int32Rect((int)position.X, (int)position.Y, 1, 1), pixel, 4, 0);

            // Convert to Color
            Color selectedColor = Color.FromArgb(pixel[3], pixel[2], pixel[1], pixel[0]);

            // Show selected color
            SelectedColorText.Text = $"Selected Color: {selectedColor}";
            SelectedColorText.Foreground = new SolidColorBrush(selectedColor);

            // Close popup
            ColorPopup.IsOpen = false;
        }
    }
}
