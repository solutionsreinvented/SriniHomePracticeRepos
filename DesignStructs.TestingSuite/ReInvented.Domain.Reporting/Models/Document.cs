using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

using Newtonsoft.Json;

using ReInvented.DataAccess.Factories;
using ReInvented.DataAccess.Interfaces;
using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Models
{
    public class Document : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public Document()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public string Number { get => Get<string>(); set => Set(value); }

        public string Title { get => Get<string>(); set => Set(value); }

        public string RevisionHistoryFilePath { get => Get<string>(); set { Set(value); RetrieveOrGenerateRevisionHistory(); } }

        [JsonProperty]
        public ObservableCollection<Revision> Revisions { get => Get<ObservableCollection<Revision>>(); private set => Set(value); }

        [JsonIgnore]
        public Revision SelectedRevision
        {
            get => Get<Revision>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(LastRevisionSelected), nameof(HasARevisionSelected));
                if (value != null)
                {
                    value.PropertyChanged -= OnSelectedRevisionPropertyChanged;
                    value.PropertyChanged += OnSelectedRevisionPropertyChanged;
                }
            }
        }

        private void OnSelectedRevisionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Revision revision = sender as Revision;

            if (e.PropertyName == nameof(Revision.Code))
            {
                Revision.CoerceRevisionCode(revision, Revisions);
            }
        }

        [JsonIgnore]
        public bool LastRevisionSelected => Revisions != null && Revisions.Count > 0 && Revisions.Last() == SelectedRevision;

        [JsonIgnore]
        public bool HasARevisionSelected => SelectedRevision != null;

        #endregion

        #region Commands

        [JsonIgnore]
        public ICommand SelectRevisionHistoryFileCommand { get => Get<ICommand>(); private set => Set(value); }

        [JsonIgnore]
        public ICommand AddNewRevisionCommand { get => Get<ICommand>(); private set => Set(value); }

        [JsonIgnore]
        public ICommand RemoveSelectedRevisionCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSelectRevisionHistoryFile()
        {
            FileFilter filter = new FileFilter("Revision History Files", FileExtensions.RevisionHistoryFile);
            RevisionHistoryFilePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(filter);
        }

        private void OnAddNewRevision()
        {
            char sequentialRevisionCode = Revision.GetSequentialRevisionCode(Revisions);

            Revision revision = new Revision() { Code = sequentialRevisionCode };
            Revisions.Add(revision);
            SelectedRevision = revision;
        }

        private void OnRemoveSelectedRevision()
        {
            _ = Revisions.Remove(SelectedRevision);
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Revisions = new ObservableCollection<Revision>();
            SelectedRevision = Revisions.FirstOrDefault();
            SelectRevisionHistoryFileCommand = new RelayCommand(OnSelectRevisionHistoryFile, true);
            AddNewRevisionCommand = new RelayCommand(OnAddNewRevision, true);
            RemoveSelectedRevisionCommand = new RelayCommand(OnRemoveSelectedRevision, true);
        }

        private void RetrieveOrGenerateRevisionHistory()
        {
            if (!string.IsNullOrWhiteSpace(RevisionHistoryFilePath) && File.Exists(RevisionHistoryFilePath))
            {
                IDataSerializer<ObservableCollection<Revision>> serializer = SerializerFactory.GetSerializer<ObservableCollection<Revision>>();
                Revisions = serializer.Deserialize(RevisionHistoryFilePath);

                SelectedRevision = Revisions.FirstOrDefault();
            }
        }



        #endregion

    }
}
