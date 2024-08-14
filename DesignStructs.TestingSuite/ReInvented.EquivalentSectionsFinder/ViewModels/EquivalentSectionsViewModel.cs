using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ReInvented.EquivalentSectionsFinder.Models;
using ReInvented.EquivalentSectionsFinder.Services;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.EquivalentSectionsFinder.ViewModels
{
    public class EquivalentSectionsViewModel : ValidatablePropertyStore
    {
        #region Default Constructor

        public EquivalentSectionsViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public EquivalentSectionsService EquivalentSectionsService { get; set; }

        public IEnumerable<SelectableDatabase> Databases => SectionsRepository.Instance.GetSectionsLibrary().Databases.Select(db => new SelectableDatabase(db));

        public SelectableDatabase SelectedDatabase { get => Get<SelectableDatabase>(); set { Set(value); SelectedShape = SelectedDatabase?.SectionShapes.FirstOrDefault(); } }

        public SectionShape SelectedShape { get => Get<SectionShape>(); set { Set(value); SelectedClassification = SelectedShape?.Classifications.FirstOrDefault(); } }

        public Classification SelectedClassification { get => Get<Classification>(); set { Set(value); SelectedSection = SelectedClassification?.Sections.FirstOrDefault(); } }

        public IRolledSection SelectedSection { get => Get<IRolledSection>(); set => Set(value); }

        public SectionPropertyComparisonResult SelectedComparisonResult { get => Get<SectionPropertyComparisonResult>(); set { Set(value); RaisePropertyChanged(nameof(HasASelectedResult)); } }

        public double PercentDifference { get => Get<double>(); set => Set(value.InPercentage()); }

        public double MatchProbabilityPercent { get => Get<double>(); set => Set(value.InPercentage()); }


        public ObservableCollection<SelectableDatabase> LookInDatabases { get => Get<ObservableCollection<SelectableDatabase>>(); set => Set(value); }

        public ObservableCollection<SectionPropertyComparisonResult> ComparisonResults { get => Get<ObservableCollection<SectionPropertyComparisonResult>>(); private set => Set(value); }

        public bool HasValidLookInDatabases => LookInDatabases != null && LookInDatabases.Count >= 1;

        public bool HasASelectedResult => SelectedComparisonResult != null;

        #endregion

        #region Commands

        public ICommand ToggleDatabaseCommand { get; private set; }

        public ICommand CheckForEquivalentSectionsCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnToggleDatabaselection(SelectableDatabase database)
        {
            if (database.IsSelected)
            {
                LookInDatabases.Add(database);
            }
            else
            {
                _ = LookInDatabases.Remove(database);
            }
        }

        private void OnCheckForEquivalentSections()
        {
            var equivalentSectionsResults = EquivalentSectionsService.GetEquivalentSections(SelectedSection, LookInDatabases, PercentDifference, MatchProbabilityPercent);
            ComparisonResults = new ObservableCollection<SectionPropertyComparisonResult>(equivalentSectionsResults);

            if (ComparisonResults.Count() <= 0)
            {
                MessageBox.Show("No equivalent sections found!", "Equivalent sections");
            }
        }

        #endregion

        #region Event Handlers

        private void OnLookInDatabasesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(HasValidLookInDatabases));
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            EquivalentSectionsService = new EquivalentSectionsService();
            SelectedDatabase = Databases.FirstOrDefault();
            LookInDatabases = new ObservableCollection<SelectableDatabase>();
            PercentDifference = 5;
            MatchProbabilityPercent = 100;

            LookInDatabases.CollectionChanged -= OnLookInDatabasesCollectionChanged;
            LookInDatabases.CollectionChanged += OnLookInDatabasesCollectionChanged;

            ToggleDatabaseCommand = new RelayCommand<SelectableDatabase>(OnToggleDatabaselection);
            CheckForEquivalentSectionsCommand = new RelayCommand(OnCheckForEquivalentSections, true);
        }

        #endregion
    }
}
