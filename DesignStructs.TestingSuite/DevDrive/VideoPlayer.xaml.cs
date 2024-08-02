using System;
using System.Windows;
using System.Windows.Input;

namespace DevDrive
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        public VideoPlayer()
        {
            InitializeComponent();

            string videoPath = @"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\Help\Videos\modify-load-combinations-file.mp4";
            mediaElement.Source = new Uri(videoPath);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) => mediaElement.Play();

        private void PauseButton_Click(object sender, RoutedEventArgs e) => mediaElement.Pause();

        private void StopButton_Click(object sender, RoutedEventArgs e) => mediaElement.Stop();

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Maximized;

        private void RestoreButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Normal;

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void OnMouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}

