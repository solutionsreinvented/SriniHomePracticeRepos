using ReInvented.Documentation.Reporting.Enums;
using ReInvented.Documentation.Reporting.Models;

namespace ReInvented.Documentation.Reporting.Interfaces
{
    public interface IProject
    {
        ProjectType Type { get; set; }

        string Code { get; set; }

        string Name { get; set; }

        string Client { get; set; }

        string ReferenceDocument { get; set; }

        Scrutinizer Originator { get; set; }

        Scrutinizer Reviewer { get; set; }

        Scrutinizer Approver { get; set; }
    }
}
