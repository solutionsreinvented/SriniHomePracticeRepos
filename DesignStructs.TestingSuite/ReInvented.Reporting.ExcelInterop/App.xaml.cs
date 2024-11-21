using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;
using Microsoft.Playwright;
using ReInvented.DataAccess;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Reporting.ExcelInterop.Models;
using ReInvented.Reporting.ExcelInterop.Services;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            _ = GenerateExcelDocument();

            //MainWindow = new MainWindow(fldWorkbook);/// { DataContext = new WebViewViewModel(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.html") };

            //MainWindow.Show();


            //base.OnStartup(e);
        }

        private static async Task GenerateExcelDocument()
        {
            HtmlRenderer htmlRenderer = new HtmlRenderer();
            HtmlDocument htmlDocument = await htmlRenderer.RenderHtmlAsync(@"F:\06. ReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Reports\Templates\Pages\testing.html");

            JsonDataSerializer<FLDReport> serializer = new JsonDataSerializer<FLDReport>();
            FLDReport fldReport = serializer.Deserialize(@"C:\Users\srini\source\repos\DesignStructs.TestingSuite\ReInvented.Reporting.ExcelInterop\Data\FLD.json");

            FldWorkbook fldWorkbook = new FldWorkbook(@"C:\Users\srini\Desktop", "Exported Data1.xlsm", fldReport);
            //fldWorkbook.Create();
        }

    }
}
