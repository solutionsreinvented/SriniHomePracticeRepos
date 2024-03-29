﻿
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

        private readonly Block _block;

        #endregion

        #region Constructors

        private Flat()
        {
            Description = $"{_block?.Name}{Number}";
        }

        public Flat(Block block, string ownedBy)
            : this(block, ownedBy, DateTime.Today)
        {

        }

        public Flat(Block block, string ownedBy, DateTime accountOpenedOn)
            : this()
        {
            _block = block;
            OwnedBy = ownedBy;
            AccountOpenedOn = accountOpenedOn;
        }

        public Flat(Block block, string ownedBy, string coOwnedBy)
            : this(block, ownedBy, coOwnedBy, DateTime.Today)
        {

        }

        public Flat(Block block, string ownedBy, string coOwnedBy, DateTime accountOpenedOn)
            : this()
        {
            _block = block;
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
        public DateTime DateSpecified { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(OutstandingOnSpecifiedDate)); } }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public decimal OpeningBalance { get => Get<decimal>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the expenses occured for this flat.
        /// </summary>
        public ObservableCollection<Expense> Expenses { get => Get<ObservableCollection<Expense>>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the penalties imposed for this flat.
        /// TODO: Remove the JsonIgnore and XmlIgnore incase the penalties data has to be persisted.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public ObservableCollection<Expense> Penalties { get => Get<ObservableCollection<Expense>>(); private set => Set(value); }
        /// <summary>
        /// Keeps track of the all the payments made by the flat owner.
        /// </summary>
        public ObservableCollection<Payment> Payments { get => Get<ObservableCollection<Payment>>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        /// <summary>
        /// Outstanding balance pending as on the selected date.
        /// </summary>
        public decimal OutstandingOnSpecifiedDate => GetOutstandingBalanceOnSpecifiedDate(DateSpecified);
        /// <summary>
        /// Outstanding balance pending calculated till date.
        /// </summary>
        public decimal CurrentOutstanding => GetOutstandingBalanceOnSpecifiedDate();
        /// <summary>
        /// Total penalty till the specified date.
        /// </summary>
        public decimal PenaltyTillSpecifiedDate => GetTotalPenaltyOnSpecifiedDate(DateSpecified);
        /// <summary>
        /// Total penalty calculated till date.
        /// </summary>
        public decimal CurrentTotalPenalty => GetTotalPenaltyOnSpecifiedDate();

        ///// <summary>
        ///// Summary of transactions since the inception of the account till the required date.
        ///// </summary>
        //public List<TransactionRecord> TransactionsSummary => ReportsProvider.GenerateTransactionHistory(this, DateSpecified);

        #endregion

        #region Public Functions

        /// <summary>
        /// Identifies if a duplicate <see cref="Expense"/> entry exists in the <see cref="Expenses"/> list of the flat.
        /// </summary>
        /// <param name="expense"><see cref="Expense"/> entry that is newly created.</param>
        /// <returns>True or False indicating if the entry exists.</returns>
        public bool ContainsSimilar(Expense expense)
        {
            return Expenses?.FirstOrDefault(e => e.Equals(expense)) != null;
        }
        /// <summary>
        /// Identifies if a duplicate <see cref="Payment"/> entry exists in the <see cref="Payments"/> list of the flat.
        /// </summary>s
        /// <param name="payment"><see cref="Payment"/> entry that is newly created.</param>
        /// <returns>True or False indicating if the entry exists.</returns>
        public bool ContainsSimilar(Payment payment)
        {
            return Payments?.FirstOrDefault(e => e.Equals(payment)) != null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified <see cref="Expense"/> entry to the flat's <see cref="Expenses"/> list and raises notifications.
        /// </summary>
        /// <param name="expense">A newly created <see cref="Expense"/> item.</param>
        public void AddExpense(Expense expense)
        {
            if (Expenses == null)
            {
                Expenses = new ObservableCollection<Expense>();
            }

            Expenses.Add(expense);
            RaiseMultiplePropertiesChanged(nameof(CurrentOutstanding), nameof(OutstandingOnSpecifiedDate));
        }
        /// <summary>
        /// Adds the specified <see cref="Payment"/> entry to the flat's <see cref="Payments"/> list and raises notifications.
        /// </summary>
        /// <param name="payment">A newly created <see cref="Payment"/> item.</param>
        public void AddPayment(Payment payment)
        {
            if (Payments == null)
            {
                Payments = new ObservableCollection<Payment>();
            }

            Payments.Add(payment);
            RaiseMultiplePropertiesChanged(nameof(CurrentOutstanding), nameof(OutstandingOnSpecifiedDate));
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

            decimal expensesTillSpecifiedDate = Expenses == null ? 0.0m : Expenses.Where(e => e.OccuredOn <= calculatedTill).Sum(e => e.Amount);

            decimal paymentsTillSpecifiedDate = Payments == null ? 0.0m : Payments.Where(p => p.ReceivedOn <= calculatedTill).Sum(p => p.Amount);

            decimal penaltiesTillSpecifiedDate = Penalties == null || Penalties.Count() == 0 ? 0.0m :
                                                              Penalties.Where(p => p.OccuredOn <= calculatedTill).Sum(p => p.Amount);

            decimal outstandingOnSpecifiedDate = OpeningBalance
                                                    + expensesTillSpecifiedDate
                                                    + penaltiesTillSpecifiedDate
                                                    - paymentsTillSpecifiedDate;

            return outstandingOnSpecifiedDate;
        }

        private decimal GetTotalPenaltyOnSpecifiedDate(DateTime? calculatedTill = null)
        {
            if (calculatedTill == null)
            {
                calculatedTill = DateTime.Today;
            }

            /// TODO: The below formula was used to provide the total penalty imposed till date.
            /// However, to simply reporting of penalties applicable on a given date formula is changed.
            /// Hence, if the previous functionality to be restored:
            ///     a. Uncomment the code below
            ///     b. Change the header of last column of the view to 'Total Penalty'.

            ///return Penalties == null || Penalties.Count() == 0 ? 0.0m :
            /// Penalties.Where(p => p.OccuredOn <= calculatedTill).Sum(p => p.Amount);

            Expense penaltyOnSpecifiedDate = Penalties?.FirstOrDefault(p => p.OccuredOn == DateSpecified);

            return penaltyOnSpecifiedDate == null ? 0.0m : penaltyOnSpecifiedDate.Amount;

        }

        public void GeneratePenalties(Block block)
        {
            List<DateTime> dates = DayOccurencesFinder
                                        .FindFor(block.PenaltyCommencesFrom, DateSpecified, block.PaymentCutoffDay);

            Penalties = new ObservableCollection<Expense>();

            for (int i = 0; i < dates.Count; i++)
            {
                decimal outstanding = GetOutstandingBalanceOnSpecifiedDate(dates[i]);

                Expense previousPenalty = Penalties?.FirstOrDefault(p => p.OccuredOn == dates[i - 1]);

                if (previousPenalty != null)
                {
                    outstanding += previousPenalty.Amount;
                }

                if (outstanding >= block.MinimumOutstandingForPenalty)
                {
                    Expense penalty = new Expense(TransactionCategory.PenaltyMaintenance,
                                                  outstanding * block.PenaltyPercentage, dates[i]);
                    Penalties.Add(penalty);
                }
            }
        }

        #endregion
    }
}
