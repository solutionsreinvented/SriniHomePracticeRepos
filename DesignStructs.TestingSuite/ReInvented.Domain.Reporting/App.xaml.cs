using System.Windows;

using HtmlAgilityPack;

using ReInvented.Domain.Reporting.ViewModels;
using ReInvented.Domain.Reporting.Views;

namespace ReInvented.Domain.Reporting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\Reports\23-4042\foundation-load-data.html";
            //HtmlDocument htmlDocument = new HtmlDocument();
            //htmlDocument.Load(filePath);


            base.OnStartup(e);

            MainWindow = new FoundationLoadDataView() { DataContext = new FoundationLoadDataViewModel() };
            MainWindow.Show();

            ///string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\foundation-load-data-content.js";

            ///JsonDataSerializer<FoundationLoadData> serializer = new JsonDataSerializer<FoundationLoadData>();


            ///File.WriteAllText(filePath, $"FoundationLoadData = {serializer.Serialize(foundationLoadData, JsonSerializerSettingsProvider.MinifiedSettings())}");

        }
    }
}