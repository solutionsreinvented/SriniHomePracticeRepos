using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Web.WebView2.Wpf;

namespace ReInvented.Reporting.ExcelInterop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadAndExtractHtmlContentAsync();
        }

        private async Task LoadAndExtractHtmlContentAsync()
        {
            // Create a WebView2 instance
            WebView2 webView = new WebView2();
            AddChild(webView);
            // Initialize the WebView2 environment
            await webView.EnsureCoreWebView2Async();

            // Path to the HTML file
            string htmlFilePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.html";

            if (!File.Exists(htmlFilePath))
            {
                MessageBox.Show("HTML file not found!");
                return;
            }

            // Load the HTML file into WebView2
            webView.Source = new Uri(htmlFilePath);

            // Wait until the content is fully loaded
            webView.NavigationCompleted += async (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    string svgContent = await ConvertSvgToPngAsync("supportLayoutPCD1", webView, "");
                    SvgToPngConverter.ConvertSvgToPng(svgContent, @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\36m\03. STAAD\03. Reports\3913A0TR036CV105r4.png", 600, 600);
                }
                else
                {
                    MessageBox.Show("Failed to load HTML file.");
                }
            };
        }

        private async Task<string> ConvertSvgToPngAsync(string svgId, WebView2 webView, string pngFilePath)
        {
            string script = $@"
            (function() {{
                var svgElement = document.getElementById('{svgId}');
                if (!svgElement) {{
                    return null;
                }}
                return svgElement.outerHTML;
            }})();";

            string svgContent = await webView.ExecuteScriptAsync(script);

            if (string.IsNullOrWhiteSpace(svgContent) || svgContent == "null")
            {
                throw new InvalidOperationException($"SVG element with ID '{svgId}' not found.");
            }

            //svgContent = svgContent.Trim('"').Replace("\\n", "").Replace("\\\"", "\"");

            //byte[] pngData = SvgToPngConverterLibrary.ConvertSvgToPng(svgContent);
            //File.WriteAllBytes(pngFilePath, pngData);

            return svgContent;
        }
    }
}
