using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

public static class WebView2Extensions
{
    /// <summary>
    /// Initializes a WebView2 control and loads a specified HTML file.
    /// </summary>
    /// <param name="webView">The WebView2 object to initialize.</param>
    /// <param name="htmlFilePath">The path to the HTML file to load.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<WebView2> InitializeAndLoadHtmlAsync(this WebView2 webView, string htmlFilePath)
    {
        if (webView == null)
            throw new ArgumentNullException(nameof(webView), "WebView2 object cannot be null.");

        if (string.IsNullOrWhiteSpace(htmlFilePath) || !File.Exists(htmlFilePath))
            throw new ArgumentException("Invalid HTML file path.", nameof(htmlFilePath));

        //await EnsureWebView2InitializedAsync(webView);

        string fileUri = new Uri(htmlFilePath).AbsoluteUri;
        webView.CoreWebView2.Navigate(fileUri);

        // Wait for the navigation to complete
        await WaitForNavigationAsync(webView);

        return webView;
    }

    /// <summary>
    /// Extracts an SVG element with the specified ID from the loaded WebView2 document.
    /// </summary>
    /// <param name="webView">The WebView2 object containing the loaded document.</param>
    /// <param name="id">The ID of the SVG element to extract.</param>
    /// <returns>A task that represents the asynchronous operation, with the SVG element's outer HTML as a result.</returns>
    public static async Task<string> GetSvgElementByIdAsync(this WebView2 webView, string id)
    {
        if (webView == null)
            throw new ArgumentNullException(nameof(webView));

        await EnsureWebView2InitializedAsync(webView);

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID cannot be null or empty.", nameof(id));

        string script = $@"(function() {{
                var svg = document.getElementById('{id}');
                return svg ? svg.outerHTML : null;
            }})();";

        string result = await webView.CoreWebView2.ExecuteScriptAsync(script);
        result = Regex.Unescape(result);
        return result?.Trim('"') ?? string.Empty;
    }

    /// <summary>
    /// Extracts all SVG elements from the loaded WebView2 document.
    /// </summary>
    /// <param name="webView">The WebView2 object containing the loaded document.</param>
    /// <returns>A task that represents the asynchronous operation, with all SVG elements as an array of strings.</returns>
    public static async Task<string[]> GetAllSvgElementsAsync(this WebView2 webView)
    {
        if (webView == null)
            throw new ArgumentNullException(nameof(webView));

        await EnsureWebView2InitializedAsync(webView);

        var script = @"(function() {
                const svgs = document.querySelectorAll('svg');
                return Array.from(svgs).map(svg => svg.outerHTML);
            })();";

        var result = await webView.CoreWebView2.ExecuteScriptAsync(script);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(result);
    }

    /// <summary>
    /// Retrieves elements of a specified type (e.g., SVG, Table, etc.) in a format manageable by HtmlAgilityPack.
    /// </summary>
    /// <param name="webView">The WebView2 object containing the loaded document.</param>
    /// <param name="tagName">The tag name of the elements to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, with the elements as HtmlNode objects.</returns>
    public static async Task<HtmlNodeCollection> GetElementsByTagNameAsync(this WebView2 webView, string tagName)
    {
        if (webView == null)
            throw new ArgumentNullException(nameof(webView));

        await EnsureWebView2InitializedAsync(webView);

        if (string.IsNullOrWhiteSpace(tagName))
            throw new ArgumentException("Tag name cannot be null or empty.", nameof(tagName));

        var script = $@"(function() {{
                const elements = document.querySelectorAll('{tagName}');
                return Array.from(elements).map(el => el.outerHTML);
            }})();";

        var result = await webView.CoreWebView2.ExecuteScriptAsync(script);

        if (result == null)
            return null;

        var htmlElements = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(result);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(string.Join("", htmlElements));
        return htmlDoc.DocumentNode.ChildNodes;
    }

    /// <summary>
    /// Ensures the WebView2 is initialized, even if it's not part of the visual tree.
    /// </summary>
    /// <param name="webView">The WebView2 object to initialize.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task EnsureWebView2InitializedAsync(this WebView2 webView)
    {
        if (webView == null)
            throw new ArgumentNullException(nameof(webView));

        if (webView.CoreWebView2 == null)
        {
            var environment = await CoreWebView2Environment.CreateAsync();
            await webView.EnsureCoreWebView2Async(environment);
        }
    }

    /// <summary>
    /// Waits for the WebView2 to complete navigation.
    /// </summary>
    /// <param name="webView">The WebView2 object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static Task WaitForNavigationAsync(this WebView2 webView)
    {
        var tcs = new TaskCompletionSource<bool>();
        webView.CoreWebView2.NavigationCompleted += (sender, args) =>
        {
            if (args.IsSuccess)
                tcs.TrySetResult(true);
            else
                tcs.TrySetException(new InvalidOperationException("Navigation failed."));
        };
        return tcs.Task;
    }
}
