using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Base
{
    public class Report : ErrorsEnabledPropertyStore, IReport
    {
        #region Default Constructor

        public Report()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        public Document Document { get => Get<Document>(); private set => Set(value); }

        public DataSource DataSource { get => Get<DataSource>(); private set => Set(value); }

        public IReportContent Content { get => Get<IReportContent>(); set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            ProjectInfo = new ProjectInfo() { ProjectDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\Reports\23-4042" };
            Document = new Document();
            DataSource = new DataSource();
        }

        #endregion
    }
}
