using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Interfaces;

namespace ReInvented.Domain.Optimization.Models
{
    public class PlatesOptimizationCriteria : ValidatablePropertyStore
    {
        #region Default Constructor

        public PlatesOptimizationCriteria()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public double PartialFactor { get => Get<double>(); set { Set(value); RaisePropertyChanged(nameof(LimitingStress)); } }

        public int ThreadCount { get => Get<int>(); set => Set(value); }

        public double MinimumThickness { get => Get<double>(); set => Set(value); }

        public double CorrosionAllowance { get => Get<double>(); set => Set(value); }

        public double AllowedOverstressedPlatesFraction { get => Get<double>(); set => Set(value.InPercentage()); }

        public double LimitingStress => Grade == null ? 0.0 : PartialFactor * Grade.Fy;

        public ILoadCase StartLoadCase { get => Get<ILoadCase>(); set => Set(value); }

        public ILoadCase EndLoadCase { get => Get<ILoadCase>(); set => Set(value); }

        public LoadCaseType LoadCaseType { get => Get<LoadCaseType>(); set => Set(value); }

        public IEnumerable<ILoadCase> LoadCasesRange { get => Get<IEnumerable<ILoadCase>>(); internal set => Set(value); }

        public IEnumerable<int> PossibleThreadCount => Enumerable.Range(1, 12);

        public MaterialGrade Grade { get => Get<MaterialGrade>(); set { Set(value); RaisePropertyChanged(nameof(LimitingStress)); } }

        public HashSet<string> ExcludedGroupNames { get; private set; } = new HashSet<string>() { "TANK", "COMP", "CENTRE", "LAUNDER" };

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
            LoadCaseType = LoadCaseType.LoadCombination;
            MaterialsLibrary = MaterialsRepository.Instance.GetMaterialsLibrary();
            SelectedTable = MaterialsLibrary.Tables.FirstOrDefault();
            ThreadCount = 1;
            AllowedOverstressedPlatesFraction = 0.10;
            PartialFactor = 0.90;
        }

        #endregion
    }
}
