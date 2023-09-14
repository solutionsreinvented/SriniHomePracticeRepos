using System;
using System.Security.Cryptography;
using System.Windows;

using ReInvented.StaadPro.Interactivity.Entities;

namespace Flexture.GeometricCalculations
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Simplified UserPrincipal class
        class UserPrincipal
        {
            public string GivenName { get; }
            public string Surname { get; }
            public string EmailAddress { get; }

            public UserPrincipal(string givenName, string surname, string emailAddress)
            {
                GivenName = givenName;
                Surname = surname;
                EmailAddress = emailAddress;
            }
        }

        // Simplified key generation method based on the seed
        static RSAParameters GenerateKeyPair(byte[] seed)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[16];
                rng.GetBytes(randomNumber);

                // XOR the seed with random data to generate key components
                byte[] privateExponent = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    privateExponent[i] = (byte)(seed[i] ^ randomNumber[i]);
                }

                return new RSAParameters
                {
                    Modulus = seed,
                    Exponent = new byte[] { 1, 0, 1 },
                    D = privateExponent
                };
            }
        }

        private void GatherUserInformation()
        {
            try
            {
                UserPrincipal user = new UserPrincipal("John", "Doe", "johndoe@example.com");

                // Combine common properties to create a seed
                string seed = $"{user.GivenName}{user.Surname}{user.EmailAddress}";

                // Convert seed to bytes (you might use a proper encoding)
                byte[] seedBytes = System.Text.Encoding.UTF8.GetBytes(seed);

                // Generate the keys based on the seed
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(GenerateKeyPair(seedBytes));

                    // Get the public and private keys
                    string publicKey = Convert.ToBase64String(rsa.ExportParameters(false).Modulus);
                    string privateKey = Convert.ToBase64String(rsa.ExportParameters(true).D);

                    Console.WriteLine("Public Key: " + publicKey);
                    Console.WriteLine("Private Key: " + privateKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GatherUserInformation();

            ///// Create and show the SplashScreen
            //SplashScreenView splashScreenView = new SplashScreenView() { DataContext = new SplashScreenViewModel(splashTimer) };
            //splashScreenView.Show();

            ///// Create a timer to close the SplashScreen after the specified time
            //Timer splashTimer = new Timer(3000);

            //splashTimer.Elapsed += (sender, args) =>
            //{
            //    splashTimer.Stop();

            //    /// Swap the Home with the WelcomeScreen
            //    Dispatcher.Invoke(() =>
            //    {
            //        MainWindow = new Home();
            //        MainWindow.Show();

            //        /// Close the SplashScreen
            //        splashScreenView.Close();
            //    });
            //};

            //splashTimer.Start();

            ////ChatBotFuctions();

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
