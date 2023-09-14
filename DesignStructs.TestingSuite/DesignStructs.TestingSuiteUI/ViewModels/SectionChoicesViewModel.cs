using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using DesignStructs.TestingSuiteUI.Models;
using DesignStructs.TestingSuiteUI.Services;

using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace DesignStructs.TestingSuiteUI.ViewModels
{
    public class SectionChoicesViewModel : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public SectionChoicesViewModel(ObservableCollection<SectionsChoice> seedChoices = null)
        {
            Initialize(seedChoices);
        }

        #endregion

        #region Public Properties

        public SectionsLibrary SectionsLibrary { get => Get<SectionsLibrary>(); set => Set(value); }

        public Database Database { get => Get<Database>(); set { Set(value); Shape = Database.SectionShapes.FirstOrDefault(); } }

        public SectionShape Shape { get => Get<SectionShape>(); set { Set(value); Classification = Shape.Classifications.FirstOrDefault(); } }

        public string SelectionChoiceName
        {
            get => Get<string>();
            set
            {
                Set(value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Database = SectionsLibrary.Databases.FirstOrDefault();
                }
                RaiseMultiplePropertiesChanged(nameof(EnableSelections), nameof(CanCreateSelection));
            }
        }

        public SectionsChoice SelectedChoice
        {
            get => Get<SectionsChoice>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(EnableSelections), nameof(CanAddSelection), nameof(CanRemoveAll), nameof(CanRunDesign));
            }
        }

        public Classification Classification { get => Get<Classification>(); set => Set(value); }

        public Classification RemoveClassification { get => Get<Classification>(); set { Set(value); RaisePropertyChanged(nameof(CanRemoveSelection)); } }

        public ObservableCollection<SectionsChoice> SectionsChoiceCollection { get => Get<ObservableCollection<SectionsChoice>>(); private set => Set(value); }

        #endregion

        #region Commands

        public ICommand CreateNewChoiceCommand { get; set; }

        public ICommand AddSelectedClassificationCommand { get; set; }

        public ICommand RemoveSelectedClassificationCommand { get; set; }

        public ICommand AddAllClassificationsCommand { get; set; }

        public ICommand RemoveAllClassificationsCommand { get; set; }

        public ICommand RunDesignCommand { get; set; }

        #endregion

        #region UI Controls Management

        public bool EnableSelections => !string.IsNullOrWhiteSpace(SelectionChoiceName) || SelectedChoice != null;

        public bool CanCreateSelection => !string.IsNullOrWhiteSpace(SelectionChoiceName) && !SectionsChoiceCollection.Any(scc => scc.Key == SelectionChoiceName);

        public bool CanAddSelection => SelectedChoice != null;

        public bool CanRemoveSelection => CanRemoveAll && RemoveClassification != null;

        public bool CanRemoveAll => SelectedChoice != null && SelectedChoice.Classifications.Count > 0;

        public bool ChoicesExist => SectionsChoiceCollection != null && SectionsChoiceCollection.Count > 0;

        public bool CanRunDesign => SelectedChoice != null && SelectedChoice.Classifications.Count > 0;


        #endregion

        #region Command Handlers

        private void OnCreateNewChoice()
        {
            if (!string.IsNullOrWhiteSpace(SelectionChoiceName))
            {
                if (SectionsChoiceCollection.Count > 0)
                {
                    if (!SectionsChoiceCollection.Any(c => c.Key == SelectionChoiceName))
                    {
                        SectionsChoice sectionsChoice = new SectionsChoice() { Key = SelectionChoiceName };
                        SectionsChoiceCollection.Add(sectionsChoice);
                        SelectedChoice = sectionsChoice;
                        SelectionChoiceName = string.Empty;
                    }
                }
                else
                {
                    SectionsChoice sectionsChoice = new SectionsChoice() { Key = SelectionChoiceName };
                    SectionsChoiceCollection.Add(sectionsChoice);
                    SelectedChoice = sectionsChoice;
                    SelectionChoiceName = string.Empty;
                }
                RaiseMultiplePropertiesChanged(nameof(ChoicesExist), nameof(CanRunDesign));
            }
        }

        private void OnAddSelectedClassification()
        {
            if (Classification != null)
            {
                if (!SelectedChoice.Classifications.Any(cc => cc.Category == Classification.Category))
                {
                    SelectedChoice.Classifications.Add(Classification);
                    RaiseMultiplePropertiesChanged(nameof(CanRemoveAll), nameof(CanRunDesign));
                }
            }
        }

        private void OnRemoveSelectedClassification()
        {
            if (RemoveClassification != null)
            {
                _ = SelectedChoice.Classifications.Remove(RemoveClassification);
                RaiseMultiplePropertiesChanged(nameof(CanRemoveAll), nameof(CanRunDesign));
            }
        }

        private void OnAddAllClassifications()
        {
            int additions = 0;

            if (Shape.Classifications != null && Shape.Classifications.Count > 0)
            {
                foreach (Classification c in Shape.Classifications)
                {
                    if (!SelectedChoice.Classifications.Any(cc => cc.Category == c.Category))
                    {
                        SelectedChoice.Classifications.Add(c);
                        additions++;
                    }
                }
                if (additions > 0)
                {
                    RaiseMultiplePropertiesChanged(nameof(CanRemoveAll), nameof(CanRunDesign));
                }
            }
        }

        private void OnRemoveAllClassifications()
        {
            if (SelectedChoice.Classifications != null && SelectedChoice.Classifications.Count > 0)
            {
                SelectedChoice.Classifications.Clear();
                RaiseMultiplePropertiesChanged(nameof(CanRemoveAll), nameof(CanRunDesign));
            }
        }

        private void OnRunDesign()
        {
            ExcelInteropService excelInteropService = new ExcelInteropService();
            excelInteropService.VerifyDesign(SelectedChoice.Classifications.SelectMany(c => c.Sections).ToList());
        }

        #endregion

        #region Private Helpers

        private void Initialize(ObservableCollection<SectionsChoice> sectionsChoices)
        {
            SectionsLibrary = SectionsRepository.Instance.GetSectionsLibrary();

            if (sectionsChoices == null)
            {
                SectionsChoiceCollection = new ObservableCollection<SectionsChoice>();
            }
            else
            {
                SectionsChoiceCollection = sectionsChoices;
                RaiseUIManagementPropertiesNotifications();
            }

            CreateNewChoiceCommand = new RelayCommand(OnCreateNewChoice, true);
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
            RaiseMultiplePropertiesChanged(nameof(EnableSelections), nameof(CanCreateSelection), nameof(CanAddSelection));
            RaiseMultiplePropertiesChanged(nameof(CanRemoveSelection), nameof(CanRemoveAll), nameof(ChoicesExist), nameof(CanRunDesign));
        }

        #endregion
    }
}
