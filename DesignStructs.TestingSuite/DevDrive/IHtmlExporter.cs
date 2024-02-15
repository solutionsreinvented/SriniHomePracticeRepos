using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Wpf;

namespace DevDrive
{

    public class HtmlToExcelExporter
    {
        string htmlFilePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Desktop\STAAD\Reports\fld.html";
        string pdfFilePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Desktop\STAAD\Reports\pdf.html";


        public async Task Export()
        {
            WebView2 webView = new WebView2
            {
                Source = new Uri(htmlFilePath)
            };

            await webView.EnsureCoreWebView2Async();

            await PrintHtmlToPdfAsync(webView, htmlFilePath, pdfFilePath);
        }

        public async Task PrintHtmlToPdfAsync(WebView2 webView, string htmlFilePath, string pdfFilePath)
        {
            // Check if webView is initialized
            if (webView.CoreWebView2 == null)
            {
                throw new InvalidOperationException("WebView2 control is not initialized yet.");
            }

            // Load the HTML file into the WebView2 control
            webView.CoreWebView2.Navigate("file:///" + Path.GetFullPath(htmlFilePath));

            // Wait for the page to load
            //await webView.CoreWebView2.WaitForScriptToExecuteAsync("document.readyState === 'complete'");

            // Print the contents of the WebView2 control to a PDF file
            await webView.CoreWebView2.PrintToPdfAsync(pdfFilePath);
        }

    }
}