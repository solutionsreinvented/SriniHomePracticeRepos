using ReInvented.Documentation.Reporting.Enums;
using ReInvented.Documentation.Reporting.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Documentation.Reporting.Models
{
    public class Project : ErrorsEnabledPropertyStore, IProject
    {
        #region Default Constructor

        public Project()
        {
            Type = ProjectType.Order;
            Originator = new Scrutinizer();
            Reviewer = new Scrutinizer();
            Approver = new Scrutinizer();
        }

        #endregion

        #region Public Properties

        public ProjectType Type { get => Get<ProjectType>(); set => Set(value); }

        public string Code { get => Get<string>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }

        public string Client { get => Get<string>(); set => Set(value); }

        public string ReferenceDocument { get => Get<string>(); set => Set(value); }

        public Scrutinizer Originator { get => Get<Scrutinizer>(); set => Set(value); }

        public Scrutinizer Reviewer { get => Get<Scrutinizer>(); set => Set(value); }

        public Scrutinizer Approver { get => Get<Scrutinizer>(); set => Set(value); }

        #endregion
    }
}
