﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Shared.Stores;
using ReInvented.TestingSuite;

namespace ReInvented.Documentation.Reporting.Models
{
    public class Settings : ErrorsEnabledPropertyStore
    {
        public Settings()
        {
            Initialize();
        }

        #region Time Limits

        public double FileOpeningTimeout { get => Get<double>(); set => Set(value); }

        public double AnalysisTimeout { get => Get<double>(); set => Set(value); }

        public double AutoDesignTimeout { get => Get<double>(); set => Set(value); }

        public double ModelAvailabilityCheckInterval { get => Get<double>(); set => Set(value); }

        #endregion

        #region Analysis

        public bool CanStartStaadApplication { get => Get<bool>(); set => Set(value); }

        public SilentMode SilentMode { get => Get<SilentMode>(); set => Set(value); }

        public HiddenMode HiddenMode { get => Get<HiddenMode>(); set => Set(value); }

        public WaitMode WaitMode { get => Get<WaitMode>(); set => Set(value); }

        #endregion

        #region Design

        public bool PerformAutoDesign
        {
            get => Get<bool>();
            set
            {
                Set(value);
                if (!value)
                {
                    GenerateMTO = false;
                    CheckNonPreferredSectionsAlso = false;
                }
            }
        }
        public bool CheckNonPreferredSectionsAlso { get => Get<bool>(); set => Set(value); }

        #endregion

        #region Reports

        public bool GenerateMTO
        {
            get => Get<bool>();
            set
            {
                Set(value);
                if (!value)
                {
                    IncludeMTONotes = false;
                }
            }
        }

        public bool IncludeMTONotes { get => Get<bool>(); set => Set(value); }

        public bool GenerateContentLoadsCalculationReport { get => Get<bool>(); set => Set(value); }

        public bool GenerateStructureSeismicCalculationReport { get => Get<bool>(); set => Set(value); }

        #endregion

        #region Performance

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
            CanStartStaadApplication = false;
            FileOpeningTimeout = 10.0;
            AnalysisTimeout = 120.0;
            AutoDesignTimeout = 540.0;
            ModelAvailabilityCheckInterval = 0.5;
            NumberOfThreads = PossibleNumberOfThreads.FirstOrDefault();
        }

        #endregion
    }
}
