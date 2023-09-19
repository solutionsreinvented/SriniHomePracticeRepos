using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ReInvented.ExcelInteropDesign.Models;
using ReInvented.ExcelInteropDesign.Services;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;


namespace ReInvented.ExcelInteropDesign.ViewModels
{
    public class SectionsPreferenceViewModel : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public SectionsPreferenceViewModel(ObservableCollection<SectionsPreference> seedChoices = null)
        {
            //_ = Task.Run(() => Initialize(seedChoices));
            Initialize(seedChoices);
        }

        #endregion

        #region Public Properties

        public SectionsLibrary SectionsLibrary { get => Get<SectionsLibrary>(); set => Set(value); }

        public Database Database { get => Get<Database>(); set { Set(value); Shape = Database.SectionShapes.FirstOrDefault(); } }

        public SectionShape Shape { get => Get<SectionShape>(); set { Set(value); Classification = Shape?.Classifications.FirstOrDefault(); } }

        public string SectionsPreferenceName
        {
            get => Get<string>();
            set
            {
                Set(value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Database = SectionsLibrary.Databases.FirstOrDefault();
                }
                RaiseMultiplePropertiesChanged(nameof(EnablePreferences), nameof(CanCreatePreference));
            }
        }

        public SectionsPreference SelectedSectionsPreference
        {
            get => Get<SectionsPreference>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(EnablePreferences), nameof(CanAddToPreference), nameof(CanRemoveAllFromPreference), nameof(CanRunDesign));
            }
        }

        public Classification Classification { get => Get<Classification>(); set => Set(value); }

        public Classification RemoveClassification { get => Get<Classification>(); set { Set(value); RaisePropertyChanged(nameof(CanRemoveFromPreference)); } }

        public ObservableCollection<SectionsPreference> SectionsPreferenceCollection { get => Get<ObservableCollection<SectionsPreference>>(); private set => Set(value); }

        public bool LibrariesLoaded { get => Get<bool>(); private set => Set(value); }

        #endregion

        #region Commands

        public ICommand CreateNewPreferenceCommand { get; set; }

        public ICommand AddSelectedClassificationCommand { get; set; }

        public ICommand RemoveSelectedClassificationCommand { get; set; }

        public ICommand AddAllClassificationsCommand { get; set; }

        public ICommand RemoveAllClassificationsCommand { get; set; }

        public ICommand RunDesignCommand { get; set; }

        #endregion

        #region UI Controls Management

        public bool EnablePreferences => !string.IsNullOrWhiteSpace(SectionsPreferenceName) || SelectedSectionsPreference != null;

        public bool CanCreatePreference => !string.IsNullOrWhiteSpace(SectionsPreferenceName) && !SectionsPreferenceCollection.Any(scc => scc.Key == SectionsPreferenceName);

        public bool CanAddToPreference => SelectedSectionsPreference != null;

        public bool CanRemoveFromPreference => CanRemoveAllFromPreference && RemoveClassification != null;

        public bool CanRemoveAllFromPreference => SelectedSectionsPreference != null && SelectedSectionsPreference.Classifications.Count > 0;

        public bool PreferencesExist => SectionsPreferenceCollection != null && SectionsPreferenceCollection.Count > 0;

        public bool CanRunDesign => SelectedSectionsPreference != null && SelectedSectionsPreference.Classifications.Count > 0;


        #endregion

        #region Command Handlers

        private void OnCreateNewPreference()
        {
            if (!string.IsNullOrWhiteSpace(SectionsPreferenceName))
            {
                if (SectionsPreferenceCollection.Count > 0)
                {
                    if (!SectionsPreferenceCollection.Any(c => c.Key == SectionsPreferenceName))
                    {
                        SectionsPreference preference = new SectionsPreference() { Key = SectionsPreferenceName };
                        SectionsPreferenceCollection.Add(preference);
                        SelectedSectionsPreference = preference;
                        SectionsPreferenceName = string.Empty;
                    }
                }
                else
                {
                    SectionsPreference preference = new SectionsPreference() { Key = SectionsPreferenceName };
                    SectionsPreferenceCollection.Add(preference);
                    SelectedSectionsPreference = preference;
                    SectionsPreferenceName = string.Empty;
                }
                RaiseMultiplePropertiesChanged(nameof(PreferencesExist), nameof(CanRunDesign));
            }
        }

        private void OnAddSelectedClassification()
        {
            if (Classification != null)
            {
                if (!SelectedSectionsPreference.Classifications.Any(cc => cc.Category == Classification.Category))
                {
                    SelectedSectionsPreference.Classifications.Add(Classification);
                    RaiseMultiplePropertiesChanged(nameof(CanRemoveAllFromPreference), nameof(CanRunDesign));
                }
            }
        }

        private void OnRemoveSelectedClassification()
        {
            if (RemoveClassification != null)
            {
                _ = SelectedSectionsPreference.Classifications.Remove(RemoveClassification);
                RaiseMultiplePropertiesChanged(nameof(CanRemoveAllFromPreference), nameof(CanRunDesign));
            }
        }

        private void OnAddAllClassifications()
        {
            int additions = 0;

            if (Shape.Classifications != null && Shape.Classifications.Count > 0)
            {
                foreach (Classification c in Shape.Classifications)
                {
                    if (!SelectedSectionsPreference.Classifications.Any(cc => cc.Category == c.Category))
                    {
                        SelectedSectionsPreference.Classifications.Add(c);
                        additions++;
                    }
                }
                if (additions > 0)
                {
                    RaiseMultiplePropertiesChanged(nameof(CanRemoveAllFromPreference), nameof(CanRunDesign));
                }
            }
        }

        private void OnRemoveAllClassifications()
        {
            if (SelectedSectionsPreference.Classifications != null && SelectedSectionsPreference.Classifications.Count > 0)
            {
                SelectedSectionsPreference.Classifications.Clear();
                RaiseMultiplePropertiesChanged(nameof(CanRemoveAllFromPreference), nameof(CanRunDesign));
            }
        }

        private void OnRunDesign()
        {
            IEnumerable<IRolledSection> sections = SelectedSectionsPreference.Classifications.SelectMany(c => c.Sections);

            SectionDesignData designData = new SectionDesignData()
            {
                AxialStrengthParameters = new HSectionAxialStrengthParameters() { Lus = 4.5, Anet = 0.95 },
                DesignMethod = Enums.DesignMethod.LRFD,
                MaterialGrade = new MaterialGrade() { Designation = "A36", Fy = 248.0, Fu = 410.0, StaadName = "A36" },
                MaterialTable = new MaterialTable() { Country = "American", Name = "ASTM", Group = Sections.Domain.Enums.MaterialShapeGroup.All },
                ShearStrengthParameters = new HSectionShearStrengthParameters() { Ts = 10.0, Spacing = 120.0, StiffenersConfiguration = Enums.WebTransverseStiffenersConfiguration.OneSide }
            };
        }

        #endregion

        #region Private Helpers

        private void Initialize(ObservableCollection<SectionsPreference> preferences)
        {
            SectionsLibrary = SectionsRepository.Instance.GetSectionsLibrary();

            //LibrariesLoaded = true;

            if (preferences == null)
            {
                SectionsPreferenceCollection = new ObservableCollection<SectionsPreference>();
            }
            else
            {
                SectionsPreferenceCollection = preferences;
                RaiseUIManagementPropertiesNotifications();
            }

            CreateNewPreferenceCommand = new RelayCommand(OnCreateNewPreference, true);
            AddSelectedClassificationCommand = new RelayCommand(OnAddSelectedClassification, true);
            AddAllClassificationsCommand = new RelayCommand(OnAddAllClassifications, true);
            RemoveSelectedClassificationCommand = new RelayCommand(OnRemoveSelectedClassification, true);
            RemoveAllClassificationsCommand = new RelayCommand(OnRemoveAllClassifications, true);
            RunDesignCommand = new RelayCommand(OnRunDesign, true);
        }

        /// <summary>
        /// This method is created specifically to raises notifications when the seed section choices are provided.
        /// </summary>
        private void RaiseUIManagementPropertiesNotifications()
        {
            RaiseMultiplePropertiesChanged(nameof(EnablePreferences), nameof(CanCreatePreference), nameof(CanAddToPreference));
            RaiseMultiplePropertiesChanged(nameof(CanRemoveFromPreference), nameof(CanRemoveAllFromPreference), nameof(PreferencesExist), nameof(CanRunDesign));
        }

        #endregion
    }
}
