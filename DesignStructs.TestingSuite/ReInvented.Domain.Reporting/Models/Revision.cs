using System;
using System.Collections.Generic;

using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Enums;

namespace ReInvented.Domain.Reporting.Models
{
    public class Revision
    {
        public Revision()
        {

        }

        public string Code { get; set; }

        public DateTime Date { get; set; }

        public Scrutinizer Originator { get; set; }

        public Scrutinizer Checker { get; set; }

        public Scrutinizer Approver { get; set; }

        public SubmissionCategory SubmissionCategory { get; set; }

        public HashSet<RevisionDescriptionItem> RevisionDescriptionCollection { get; set; }
    }
}
