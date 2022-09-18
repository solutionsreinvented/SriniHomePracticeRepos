﻿using System.Windows;

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
            btnMaximizeOrRestore.Content = WindowState == WindowState.Maximized ? "Restore" : "Maximize";
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnMaximizeOrRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            //if (WindowState == WindowState.Normal)
            //{
            //    Margin = new Thickness(25);
            //}

            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            btnMaximizeOrRestore.Content = WindowState == WindowState.Maximized ? "Restore" : "Maximize";
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}