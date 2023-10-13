using System.ComponentModel;

using ReInvented.Domain.ProjectSetup.Enums;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IProjectInfo : INotifyPropertyChanged
    {
        ProjectType Type { get; set; }

        string Code { get; set; }

        string Name { get; set; }

        string Client { get; set; }

        string ReferenceDocument { get; set; }

        string ProjectDirectory { get; set; }

        IScrutinizer Originator { get; }

        IScrutinizer Reviewer { get; }

        IScrutinizer Approver { get; }
    }
}
