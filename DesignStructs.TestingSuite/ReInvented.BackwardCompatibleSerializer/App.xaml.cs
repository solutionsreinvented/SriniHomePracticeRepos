using System.Windows;

using ReInvented.BackwardCompatibleSerializer.Services;
using ReInvented.ThickenerModelGenerator.UI.Models;

namespace ReInvented.BackwardCompatibleSerializer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string filePath = @"D:\02. Due\00. Projects\01. Pre-Order\70. E24090621 (Arab Potash)\05. Calculations\01. Working\D50.0H3.50S09.00OC1.309SC1.414IMP0.423CON0.0122MOT1500.json";
            JsonDataSerializer.MapPropertiesFromJson<Project>(JsonDataSerializer.Deserialize(filePath));
        }
    }
}
