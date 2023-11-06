using ReInvented.Domain.Reporting.Interfaces;

namespace ReInvented.Domain.Reporting.Models
{
    public class Report<T> where T : IReportContent
    {
        public Report()
        {

        }

        public DocumentInfo DocumentInfo { get; set; }

        public T Content { get; set; }
    }
}
