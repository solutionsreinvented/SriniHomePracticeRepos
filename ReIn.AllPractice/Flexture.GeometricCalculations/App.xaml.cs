using System;
using System.Timers;
using System.Windows;

using Flexture.GeometricCalculations.ViewModels;

using ReInvented.StaadPro.Interactivity.Entities;

namespace Flexture.GeometricCalculations
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /// Create and show the SplashScreen
            SplashScreenView splashScreenView = new SplashScreenView() { DataContext = new SplashScreenViewModel(splashTimer) };
            splashScreenView.Show();

            /// Create a timer to close the SplashScreen after the specified time
            Timer splashTimer = new Timer(3000);

            splashTimer.Elapsed += (sender, args) =>
            {
                splashTimer.Stop();

                /// Swap the Home with the WelcomeScreen
                Dispatcher.Invoke(() =>
                {
                    MainWindow = new Home();
                    MainWindow.Show();

                    /// Close the SplashScreen
                    splashScreenView.Close();
                });
            };

            splashTimer.Start();

            //ChatBotFuctions();

        }

        private static void ChatBotFuctions()
        {
            Member member1 = new Member(new Node() { X = 0.000, Y = 0.000, Z = 0.00 }, new Node() { X = 9.848, Y = 7.00, Z = -1.736 });
            Member member2 = new Member(new Node() { X = 7.878, Y = 0.00, Z = -1.389 }, new Node() { X = 0.000, Y = 10.00, Z = 0.00 });

            Node intersection = AIFunctions.LinesIntersection(member1, member2);

            Node rotatedNode = AIFunctions.RotateNode(member2.StartNode, member1.StartNode, member1.EndNode, 25.0 * Math.PI / 180.0);


            _ = MessageBox.Show($"Intersection node coordinates are X: {intersection.X:N3}; Y: {intersection.Y:N3}; Z: {intersection.Z:N3};.", "Intersect lines", MessageBoxButton.OK);
        }
    }
}
