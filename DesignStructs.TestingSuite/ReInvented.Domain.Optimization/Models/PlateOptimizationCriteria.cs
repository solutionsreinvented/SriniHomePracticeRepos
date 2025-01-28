using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Optimization.Models
{
    public class PlateOptimizationCriteria : ValidatablePropertyStore
    {
        #region Default Constructor

        public PlateOptimizationCriteria()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public IEnumerable<int> PossibleThreadCount => Enumerable.Range(1, 12);

        public int ThreadCount { get => Get<int>(); set => Set(value); }

        public double MinimumThickness { get => Get<double>(); set => Set(value); }

        public double CorrosionAllowance { get => Get<double>(); set => Set(value); }

        public double AllowedPercentPlatesExceedance { get => Get<double>(); set => Set(value); }

        public double PartialFactor { get => Get<double>(); set { Set(value); RaisePropertyChanged(nameof(LimitingStress)); } }

        public MaterialGrade Grade { get => Get<MaterialGrade>(); set { Set(value); RaisePropertyChanged(nameof(LimitingStress)); } }

        public HashSet<string> ExcludedGroupNames { get; private set; } = new HashSet<string>() { "TANK", "COMP", "CENTRE", "LAUNDER" };

        public double LimitingStress => Grade == null ? 0.0 : PartialFactor * Grade.Fy;

        #endregion

        #region Facilitating Properties

        [JsonIgnore]
        public MaterialsLibrary MaterialsLibrary { get; private set; }

        [JsonIgnore]
        public MaterialTable SelectedTable { get => Get<MaterialTable>(); set { Set(value); Grade = SelectedTable.Grades.FirstOrDefault(); } }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            MaterialsLibrary = MaterialsRepository.Instance.GetMaterialsLibrary();
            SelectedTable = MaterialsLibrary.Tables.FirstOrDefault();
            ThreadCount = 1;
            AllowedPercentPlatesExceedance = 0.10;
            PartialFactor = 0.90;
        }

        #endregion
    }
}
