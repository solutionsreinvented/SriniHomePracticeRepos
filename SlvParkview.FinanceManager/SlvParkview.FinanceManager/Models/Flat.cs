
using ReInvented.Shared.Stores;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using SlvParkview.FinanceManager.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;
using SlvParkview.FinanceManager.Services;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Models
{
    public class Flat : PropertyStore
    {
        #region Private Fields

        private readonly string _blockName;

        #endregion

        #region Constructors

        private Flat()
        {
            Description = $"{_blockName}{Number}";
        }

        public Flat(string blockName, string ownedBy)
            : this(blockName, ownedBy, DateTime.Today)
        {

        }

        public Flat(string blockName, string ownedBy, DateTime accountOpenedOn)
            : this()
        {
            _blockName = blockName;
            OwnedBy = ownedBy;
            AccountOpenedOn = accountOpenedOn;
        }

        public Flat(string blockName, string ownedBy, string coOwnedBy)
            : this(blockName, ownedBy, coOwnedBy, DateTime.Today)
        {

        }

        public Flat(string blockName, string ownedBy, string coOwnedBy, DateTime accountOpenedOn) : this()
        {
            _blockName = blockName;

            OwnedBy = ownedBy;
            CoOwnedBy = coOwnedBy;
            AccountOpenedOn = accountOpenedOn;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Flat number.
        /// </summary>
        public string Number { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Provides the complete description of the flat.
        /// </summary>
        public string Description { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Primary owner of the flat.
        /// </summary>
        public string OwnedBy { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Name of the joint owner if the flat is owned by more than person.
        /// </summary>
        public string CoOwnedBy { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Name of the tenant if the flat is not occupied by the owner(s).
        /// </summary>
        public string TenantName { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Indicates whether the use can modify the selected flat details.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public bool ChangesAllowed { get => Get(false); set { Set(value); RaisePropertyChanged(nameof(CanChangeTenantName)); } }
        /// <summary>
        /// Allows to change/enter the tenant name only when the <see cref="ResidentType"/> is a <see cref="ResidentType.Tenant"/>.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public bool CanChangeTenantName => ChangesAllowed && ResidentType == ResidentType.Tenant;
        /// <summary>
        /// Indicates whether flat is occupied by the owner(s) or a tenant.
        /// </summary>
        public ResidentType ResidentType
        {
            get => Get(ResidentType.Owner);
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangeTenantName));
            }
        }
        /// <summary>
        /// Date on which the flat account is started.
        /// </summary>
        public DateTime AccountOpenedOn { get => Get<DateTime>(); set => Set(value); }
        /// <summary>
        /// Date for which the results to be calculated.
        /// </summary>
        public DateTime DateSpecified { get => Get(DateTime.Today); set { Set(value); OutstandingOnSpecifiedDate = GetOutstandingBalanceOnSpecifiedDate(DateSpecified); } }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public decimal OpeningBalance { get => Get<decimal>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the expenses occured for this flat.
        /// </summary>
        public ObservableCollection<Bill> Bills { get => Get<ObservableCollection<Bill>>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the penalties imposed for this flat.
        /// TODO: Remove the JsonIgnore and XmlIgnore incase the penalties data has to be persisted.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public ObservableCollection<Bill> Penalties { get => Get<ObservableCollection<Bill>>(); private set => Set(value); }
        /// <summary>
        /// Keeps track of the all the payments made by the flat owner.
        /// </summary>
        public ObservableCollection<Receipt> Receipts { get => Get<ObservableCollection<Receipt>>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        /// <summary>
        /// Outstanding balance pending as on the selected date.
        /// </summary>
        public decimal OutstandingOnSpecifiedDate { get => Get<decimal>(); private set => Set(value); }
        /// <summary>
        /// Outstanding balance pending calculated till date.
        /// </summary>
        public decimal CurrentOutstanding { get => Get<decimal>(); private set => Set(value); }

        #endregion

        #region Public Functions

        /// <summary>
        /// Identifies if a duplicate <see cref="Bill"/> entry exists in the <see cref="Bills"/> list of the flat.
        /// </summary>
        /// <param name="expense"><see cref="Bill"/> entry that is newly created.</param>
        /// <returns>True or False indicating if the entry exists.</returns>
        public bool ContainsSimilar(Bill expense)
        {
            return Bills?.FirstOrDefault(e => e.Equals(expense)) != null;
        }
        /// <summary>
        /// Identifies if a duplicate <see cref="Receipt"/> entry exists in the <see cref="Receipts"/> list of the flat.
        /// </summary>s
        /// <param name="payment"><see cref="Receipt"/> entry that is newly created.</param>
        /// <returns>True or False indicating if the entry exists.</returns>
        public bool ContainsSimilar(Receipt payment)
        {
            return Receipts?.FirstOrDefault(e => e.Equals(payment)) != null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified <see cref="Bill"/> entry to the flat's <see cref="Bills"/> list and raises notifications.
        /// </summary>
        /// <param name="expense">A newly created <see cref="Bill"/> item.</param>
        public void AddBill(Bill expense)
        {
            if (Bills == null)
            {
                Bills = new ObservableCollection<Bill>();
            }

            Bills.Add(expense);
            UpdateOutstandings();
        }

        private void UpdateOutstandings()
        {
            CurrentOutstanding = GetOutstandingBalanceOnSpecifiedDate();
            OutstandingOnSpecifiedDate = GetOutstandingBalanceOnSpecifiedDate(DateSpecified);
        }

        /// <summary>
        /// Adds the specified <see cref="Receipt"/> entry to the flat's <see cref="Receipts"/> list and raises notifications.
        /// </summary>
        /// <param name="payment">A newly created <see cref="Receipt"/> item.</param>
        public void AddReceipt(Receipt payment)
        {
            if (Receipts == null)
            {
                Receipts = new ObservableCollection<Receipt>();
            }

            Receipts.Add(payment);
            UpdateOutstandings();
        }

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return $"{Number}{OwnedBy}{CoOwnedBy}".GetHashCode();
        }

        public bool Equals(Flat flat)
        {
            return flat != null &&
                   Description == flat.Description && OwnedBy == flat.OwnedBy &&
                   CoOwnedBy == flat.CoOwnedBy && TenantName == flat.TenantName &&
                   ResidentType == flat.ResidentType && Number == flat.Number;
        }


        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is Flat flat && Equals(flat);
        }

        public static bool operator ==(Flat a, Flat b)
        {
            return ReferenceEquals(a, b) || (!(a is null) && !(b is null) && a.Equals(b));
        }

        public static bool operator !=(Flat a, Flat b)
        {
            return !(a == b);
        }

        #endregion

        #region Helper Methods

        private decimal GetOutstandingBalanceOnSpecifiedDate(DateTime? calculatedTill = null)
        {
            if (calculatedTill == null)
            {
                calculatedTill = DateTime.Today;
            }

            decimal expensesTillSpecifiedDate = Bills == null ? 0.0m : Bills.Where(e => e.OccuredOn <= calculatedTill).Sum(e => e.Amount);

            decimal paymentsTillSpecifiedDate = Receipts == null ? 0.0m : Receipts.Where(p => p.ReceivedOn <= calculatedTill).Sum(p => p.Amount);

            decimal penaltiesTillSpecifiedDate = Penalties == null || Penalties.Count() == 0 ? 0.0m :
                                                              Penalties.Where(p => p.OccuredOn <= calculatedTill).Sum(p => p.Amount);

            decimal outstandingOnSpecifiedDate = OpeningBalance
                                                    + expensesTillSpecifiedDate
                                                    + penaltiesTillSpecifiedDate
                                                    - paymentsTillSpecifiedDate;

            return outstandingOnSpecifiedDate;
        }

        public void GeneratePenalties(PenaltyCriteria criteria)
        {
            List<DateTime> dates = DayOccurencesFinder
                                        .FindFor(criteria.CommencesFrom, DateSpecified, criteria.ReceiptCutoffDay);

            Penalties = new ObservableCollection<Bill>();

            for (int i = 0; i < dates.Count; i++)
            {
                decimal outstanding = GetOutstandingBalanceOnSpecifiedDate(dates[i]);

                Bill previousPenalty = Penalties?.FirstOrDefault(p => p.OccuredOn == dates[i - 1]);

                if (previousPenalty != null)
                {
                    outstanding += previousPenalty.Amount;
                }

                if (outstanding >= criteria.MinimumOutstanding)
                {
                    Bill penalty = new Bill(TransactionCategory.PenaltyMaintenance,
                                                  outstanding * criteria.Percentage, dates[i]);
                    Penalties.Add(penalty);
                }
            }
        }

        #endregion
    }
}
