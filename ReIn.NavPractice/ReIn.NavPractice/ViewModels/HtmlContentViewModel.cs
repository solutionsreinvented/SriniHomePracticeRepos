using System.Windows.Documents;

namespace ReIn.NavPractice.ViewModels
{
    public class HtmlContentViewModel
    {
        public HtmlContentViewModel()
        {
            Document = new FlowDocument();
        }

        public FlowDocument Document { get; set; }

        public string HtmlFilePath { get; set; } = @"C:\Users\masanams\Desktop\Desktop\HTML Tables\Index.html";
    }
}
