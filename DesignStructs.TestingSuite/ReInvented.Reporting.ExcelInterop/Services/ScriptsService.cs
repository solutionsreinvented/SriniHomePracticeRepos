namespace ReInvented.Reporting.ExcelInterop.Services
{
    public class ScriptsService
    {
        public static string AllSvgsFrom(string element)
        {
            return @"(function() {
                    const svgs = element.querySelectorAll('svg');
                    return Array.from(svgs).map(svg => svg.outerHTML);
                })();";
        }
    }
}
