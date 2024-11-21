using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using HtmlAgilityPack;

using Microsoft.Office.Interop.Excel;
using Microsoft.Web.WebView2.Wpf;

using ReInvented.Reporting.ExcelInterop.Extensions;
using ReInvented.Reporting.ExcelInterop.Models;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow(FldWorkbook fldWorkbook)
        {
            FldWorkbook = fldWorkbook;

            InitializeComponent();
            _ = InitializeAsync();
        }

        private FldWorkbook FldWorkbook { get; set; }

        private async Task InitializeAsync()
        {
            await LoadAndExtractHtmlContentAsync();
        }

        private async Task LoadAndExtractHtmlContentAsync()
        {
            string htmlFilePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\SvgToPng\03. Reports\3913A0TR036CV101r4.html";
            string directory = Path.GetDirectoryName(htmlFilePath);
            string fileName = Path.GetFileNameWithoutExtension(htmlFilePath);

            WebView2 webView = new WebView2();
            AddChild(webView);
            await webView.EnsureCoreWebView2Async();

            if (!File.Exists(htmlFilePath))
            {
                _ = MessageBox.Show("HTML file not found!");
                return;
            }

            webView.Source = new Uri(htmlFilePath);

            webView.NavigationCompleted += async (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    //FldWorkbook.InstantiateObjects();
                    FldWorkbook.Create();
                    Worksheet worksheet = FldWorkbook.Worksheet;
                    string[] svgs = await webView.GetAllSvgElementsAsync();

                    worksheet.EmbedPngToExcelFromMemory(SvgToPngConverter.ConvertSvgToPng(svgs.First(), 600, 600), worksheet.Range(1,1), worksheet.Range(35, 35));


                    HtmlNodeCollection svgElems = await webView.GetElementsByTagNameAsync("table");
                    //svgs.ToList().ForEach()


                    string svgContent = await webView.GetSvgElementByIdAsync("supportLayoutPCD1"); ///await ConvertSvgToPngAsync("supportLayoutPCD1", webView, "");
                    SvgToPngConverter.ConvertSvgToPng(svgContent, Path.Combine(directory, $"{fileName}.png"), 600, 600);
                }
                else
                {
                    _ = MessageBox.Show("Failed to load HTML file.");
                }
            };
        }
       
    }
}
