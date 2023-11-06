using System.Collections.Generic;
using System.IO;

using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;
using ReInvented.Domain.Reporting.Enums;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Models
{
    public class DocumentInfo : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public DocumentInfo()
        {
            SubmissionCategory = SubmissionCategory.Approval;
        }

        #endregion

        #region Public Properties

        public string Number { get => Get<string>(); set => Set(value); }

        public string Title { get => Get<string>(); set => Set(value); }

        public string RevisionHistoryFilePath { get => Get<string>(); set { Set(value); RetrieveOrGenerateRevisionHistory(); } }

        public SubmissionCategory SubmissionCategory { get => Get<SubmissionCategory>(); set => Set(value); }

        public HashSet<Revision> Revisions { get => Get<HashSet<Revision>>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private void RetrieveOrGenerateRevisionHistory()
        {
            if (!string.IsNullOrWhiteSpace(RevisionHistoryFilePath) && File.Exists(RevisionHistoryFilePath))
            {
                IDataSerializer<HashSet<Revision>> serializer = SerializerFactory.GetSerializer<HashSet<Revision>>();
                Revisions = serializer.Deserialize(RevisionHistoryFilePath);
            }
        }

        #endregion
    }
}
