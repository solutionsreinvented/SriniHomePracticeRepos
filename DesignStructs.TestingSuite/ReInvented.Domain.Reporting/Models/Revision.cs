using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Newtonsoft.Json;

using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Enums;
using ReInvented.Domain.Reporting.Extensions;
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

        public char Code { get => Get<char>(); set => Set(value); }

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

        #region Public Static Functions

        public static Revision GetPenultimateRevision(IEnumerable<Revision> revisions)
        {
            return revisions == null || revisions.Count() < 2 ? null : revisions.ToList()[revisions.Count() - 2];
        }

        public static char GetSequentialRevisionCode(IEnumerable<Revision> revisions)
        {
            return GetSequentialRevisionCode(revisions.LastOrDefault());
        }

        public static void CoerceRevisionCode(Revision revision, IEnumerable<Revision> revisions)
        {
            IEnumerable<Revision> revisionsExceptLast = revisions.Take(revisions.Count() - 1);

            Revision lastRevision = GetPenultimateRevision(revisions);

            if (lastRevision != null)
            {
                char lastCode = lastRevision.Code;
                char currentCode = revision.Code;

                if (lastCode.IsDigit())
                {
                    if (currentCode.IsLetter())
                    {
                        revision.Code = GetSequentialRevisionCode(lastRevision);
                    }
                }
                else
                {
                    if (currentCode.IsDigit() && Convert.ToInt32(currentCode) > Convert.ToInt32('0'))
                    {
                        revision.Code = '0';
                    }
                    if (revisionsExceptLast.Select(r => r.Code).Contains(currentCode))
                    {
                        revision.Code = GetSequentialRevisionCode(lastRevision);
                    }
                }

                if ((lastCode.IsLetter() && currentCode.IsLetter()) || (lastCode.IsDigit() && currentCode.IsDigit()))
                {
                    if (revision.Code != GetSequentialRevisionCode(lastRevision))
                    {
                        revision.Code = GetSequentialRevisionCode(lastRevision);
                    }
                }
            }

            if (revisions.Count() == 1)
            {
                if (revision.Code.IsDigit() && revision.Code != '0')
                {

                }
            }

        }


        public static char GetSequentialRevisionCode(Revision lastRevision)
        {
            char nextCode = 'A';

            if (lastRevision != null)
            {
                // Determine the type of the last revision
                bool isAlphabetical = char.IsLetter(lastRevision.Code);

                // Increment the last revision based on its type
                if (isAlphabetical)
                {
                    // If it's alphabetical, increment it to the next alphabet
                    char nextAlphabet = (char)(lastRevision.Code + 1);
                    nextCode = nextAlphabet;
                }
                else
                {
                    // If it's numeric, increment the number by one
                    int numericValue = int.Parse(lastRevision.Code.ToString());
                    numericValue++;
                    nextCode = numericValue.ToString()[0];
                }
            }

            return nextCode;
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

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
