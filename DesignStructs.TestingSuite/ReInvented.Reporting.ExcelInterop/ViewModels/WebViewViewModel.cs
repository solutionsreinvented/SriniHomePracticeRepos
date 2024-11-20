using ReInvented.Shared.Stores;

namespace ReInvented.Reporting.ExcelInterop.ViewModels
{
    public class WebViewViewModel : ValidatablePropertyStore
    {
        public WebViewViewModel(string sourceHtml)
        {
            SourceHtml = sourceHtml;
        }

        public string SourceHtml { get => Get<string>(); set => Set(value); }
    }
}
