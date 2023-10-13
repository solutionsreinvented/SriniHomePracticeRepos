using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class PerformanceSettings : ErrorsEnabledPropertyStore, IPerformanceSettings
    {
        #region Default Constructor

        public PerformanceSettings()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public bool UseMultipleThreads
        {
            get => Get<bool>();
            set
            {
                Set(value);
                if (!value)
                {
                    NumberOfThreads = PossibleNumberOfThreads.FirstOrDefault();
                }
            }
        }

        public IEnumerable<int> PossibleNumberOfThreads => Enumerable.Range(1, 12);

        public int NumberOfThreads { get => Get<int>(); set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {

            NumberOfThreads = PossibleNumberOfThreads.FirstOrDefault();
        }

        #endregion
    }
}
