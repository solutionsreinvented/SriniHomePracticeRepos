using System;

using Newtonsoft.Json;

using ReInvented.Domain.ProjectSetup.Enums;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class ProjectInfo : ErrorsEnabledPropertyStore, IProjectInfo
    {
        #region Default Constructor

        public ProjectInfo()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public ProjectType Type { get => Get<ProjectType>(); set => Set(value); }

        public string Code { get => Get<string>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }

        public string Client { get => Get<string>(); set => Set(value); }

        public string ReferenceDocument { get => Get<string>(); set => Set(value); }

        public string ProjectDirectory { get => Get<string>(); set => Set(value); }

        [JsonProperty]
        public IScrutinizer Originator { get => Get<IScrutinizer>(); private set => Set(value); }

        [JsonProperty]
        public IScrutinizer Reviewer { get => Get<IScrutinizer>(); private set => Set(value); }

        [JsonProperty]
        public IScrutinizer Approver { get => Get<IScrutinizer>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Type = ProjectType.Order;
            Originator = new Scrutinizer();
            Reviewer = new Scrutinizer();
            Approver = new Scrutinizer();
            Code = $"{DateTime.Today.Year % 100:00}-NNNN";
        }

        #endregion
    }
}
