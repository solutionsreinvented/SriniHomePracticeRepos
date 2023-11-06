using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Models
{
    public class Report<T> : ErrorsEnabledPropertyStore, IReport<T>
    {
        #region Default Constructor

        public Report()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        public DocumentInfo DocumentInfo { get => Get<DocumentInfo>(); private set => Set(value); }

        public DataSourceInformation DataSourceInformation { get => Get<DataSourceInformation>(); private set => Set(value); }

        public T Content { get; set; }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            ProjectInfo = new ProjectInfo();
            DocumentInfo = new DocumentInfo();
            DataSourceInformation = new DataSourceInformation();
        } 

        #endregion

    }
}
