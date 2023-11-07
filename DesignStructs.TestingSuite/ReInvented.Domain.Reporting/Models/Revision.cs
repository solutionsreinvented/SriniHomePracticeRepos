using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Newtonsoft.Json;

using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Enums;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Models
{
    public class Revision : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public Revision()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public string Code { get => Get<string>(); set => Set(value); }

        public DateTime Date { get => Get<DateTime>(); set => Set(value); }

        public ScrutinyHistory ScrutinyHistory { get => Get<ScrutinyHistory>(); set => Set(value); }

        public SubmissionCategory SubmissionCategory { get => Get<SubmissionCategory>(); set => Set(value); }

        [JsonProperty]
        public ObservableCollection<RevisionDescriptionItem> RevisionDescriptionItems { get => Get<ObservableCollection<RevisionDescriptionItem>>(); private set => Set(value); }

        [JsonIgnore]
        public RevisionDescriptionItem SelectedRevisionDescriptionItem
        {
            get => Get<RevisionDescriptionItem>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(HasARevisionDescriptionSelected));
            }
        }

        [JsonIgnore]
        public bool HasARevisionDescriptionSelected => SelectedRevisionDescriptionItem != null;


        #endregion

        #region Commands

        [JsonIgnore]
        public ICommand AddNewRevisionDescriptionItemCommand { get => Get<ICommand>(); private set => Set(value); }


        [JsonIgnore]
        public ICommand RemoveRevisionDescriptionItemCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            SubmissionCategory = SubmissionCategory.Approval;
            ScrutinyHistory = new ScrutinyHistory();
            RevisionDescriptionItems = new ObservableCollection<RevisionDescriptionItem>();
            AddNewRevisionDescriptionItemCommand = new RelayCommand(OnAddNewRevisionDescriptionItem, true);
            RemoveRevisionDescriptionItemCommand = new RelayCommand(OnRemoveRevisionDescriptionItem, true);
        }

        private void OnAddNewRevisionDescriptionItem()
        {
            RevisionDescriptionItem item = new RevisionDescriptionItem() { Section = "Section", Description = "-" };

            if (!RevisionDescriptionItems.Contains(item))
            {
                RevisionDescriptionItems.Add(item);
                SelectedRevisionDescriptionItem = item;
            }
        }

        private void OnRemoveRevisionDescriptionItem()
        {
            if (SelectedRevisionDescriptionItem != null && RevisionDescriptionItems.Contains(SelectedRevisionDescriptionItem))
            {
                _ = RevisionDescriptionItems.Remove(SelectedRevisionDescriptionItem);
            }
        }

        #endregion

        #region Equality

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals(Revision revision)
        {
            if (revision == null)
            {
                return false;
            }

            return Code == revision.Code;
        }


        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!(obj is Revision revision))
            {
                return false;
            }

            return Code == revision.Code;
        }

        public static bool operator ==(Revision left, Revision right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Code == right.Code;
        }

        public static bool operator !=(Revision left, Revision right) => !(left == right);

        #endregion
    }
}
