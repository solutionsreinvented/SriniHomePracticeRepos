using System.Collections.ObjectModel;
using System.Windows;

using ReInvented.Domain.Tass;
using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.ThickenerModelGenerator.UI.Models;

namespace DevDrive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IProject project = new Project();
            Thickener thickener = new Thickener();
            IInput input = project.Input;

            for (int rbIndex = 0; rbIndex < input.SupportStructure.NumberOfRadialBeams; rbIndex++)
            {
                ISupportStructureGridRow row = input.SupportStructure.Grids[rbIndex];

            }

            base.OnStartup(e);
            HtmlToExcelExporter exporter = new HtmlToExcelExporter();
            _ = exporter.Export();
        }
    }
}
