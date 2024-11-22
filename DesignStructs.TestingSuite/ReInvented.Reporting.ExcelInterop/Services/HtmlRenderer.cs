using HtmlAgilityPack;
using Microsoft.Playwright;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ReInvented.Reporting.ExcelInterop.Services
{
    public class HtmlRenderer
    {
        public static async Task LoadHtml(string htmlFilePath)
        {
            HtmlRenderer renderer = new HtmlRenderer();
            string filePath = Path.Combine(htmlFilePath);

            HtmlDocument htmlDocument = await renderer.RenderHtmlAsync(filePath);

            HtmlNode titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
            Debug.WriteLine($"Title: {titleNode?.InnerText}");
        }

        public async Task<HtmlDocument> RenderHtmlAsync(string filePath)
        {
            IPlaywright playwright = null;
            try
            {
                playwright = await Playwright.CreateAsync();
                IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

                IPage page = await browser.NewPageAsync();
                await page.GotoAsync($"file:///{filePath.Replace("\\", "/")}"); // Ensure proper file URI format

                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

                string renderedHtml = await page.ContentAsync();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(renderedHtml);

                await browser.CloseAsync();

                return htmlDocument;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                playwright?.Dispose();
            }
        }

        #region Static Functions

        public static void InstallBrowsers(int timeoutMilliseconds, string playwrightScriptFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(playwrightScriptFilePath) || !File.Exists(playwrightScriptFilePath))
            {
                playwrightScriptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "playwright.ps1");
            }

            // Run the PowerShell script to install browsers
            ProcessStartInfo processInfo = GetProcessStartInfo(playwrightScriptFilePath);

            using (var process = Process.Start(processInfo))
            {
                if (process == null)
                {
                    Console.WriteLine("Failed to start PowerShell process.");
                    return;
                }

                // Add a timeout to prevent indefinite hanging
                bool exited = process.WaitForExit(timeoutMilliseconds); // 60 seconds timeout

                if (!exited)
                {
                    Console.WriteLine("Process timed out. Killing process...");
                    process.Kill();
                }
                else
                {
                    // Check exit code and output
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("Playwright browsers installed successfully!");
                        Console.WriteLine(output);
                    }
                    else
                    {
                        Console.WriteLine("Playwright installation failed.");
                        Console.WriteLine($"Error: {error}");
                    }
                }
            }
        }

        private static ProcessStartInfo GetProcessStartInfo(string playwrightScriptFilePath)
        {
            return new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"-ExecutionPolicy Bypass -File \"{playwrightScriptFilePath}\" install",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
        }

        /// Worked in Home PC
        public static void InstallBrowsers(string playwrightScriptFilePath = null)
        {

            if (string.IsNullOrWhiteSpace(playwrightScriptFilePath) || !File.Exists(playwrightScriptFilePath))
            {
                playwrightScriptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "playwright.ps1");
            }

            // Run the PowerShell script to install browsers
            ProcessStartInfo processInfo = GetProcessStartInfo(playwrightScriptFilePath);

            Process process = Process.Start(processInfo);
            process.WaitForExit();

            // Log or handle success/failure
            if (process.ExitCode == 0)
            {
                Debug.Print("Playwright browsers installed successfully!");
            }
            else
            {
                Debug.Print("Failed to install Playwright browsers.");
            }
        }

        #endregion

    }
}
