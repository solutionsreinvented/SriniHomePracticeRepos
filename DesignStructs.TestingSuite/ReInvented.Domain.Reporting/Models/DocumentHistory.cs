using System.Collections.Generic;

using ReInvented.Domain.Reporting.Enums;

namespace ReInvented.Domain.Reporting.Models
{
    public class DocumentHistory
    {
        public DocumentHistory()
        {

        }

        public string Number { get; set; }

        public string ProjectNumber { get; set; }

        public SubmissionCategory SubmissionCategory { get; set; }

        public HashSet<Revision> Revisions { get; set; }
    }
}
