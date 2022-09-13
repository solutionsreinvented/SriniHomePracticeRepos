using ApartmentFinanceManager.Enums;

using Newtonsoft.Json;

using ReInvented.Shared.Stores;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApartmentFinanceManager.Models
{
    public class Flat : PropertyStore
    {
        #region Private Fields

        private readonly ApartmentBlock _block;

        #endregion

        #region Constructors

        private Flat()
        {
            Description = $"{_block?.Name}{Number}";
        }

        public Flat(ApartmentBlock block, string ownedBy)
            : this(block, ownedBy, DateTime.Today)
        {

        }

        public Flat(ApartmentBlock block, string ownedBy, DateTime accountOpenedOn)
            : this()
        {
            _block = block;
            OwnedBy = ownedBy;
            AccountOpenedOn = accountOpenedOn;
        }

        public Flat(ApartmentBlock block, string ownedBy, string coOwnedBy)
            : this(block, ownedBy, coOwnedBy, DateTime.Today)
        {

        }

        public Flat(ApartmentBlock block, string ownedBy, string coOwnedBy, DateTime accountOpenedOn)
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
        public string Description { get => Get<string>(); set => Set(value); } /// => $"{_block?.Name}{Number}";
        /// <summary>
        /// Name of the primary owner.
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
        /// Indicates whether flat is occupied by the owner(s) or a tenant.
        /// </summary>
        public ResidentType ResidentType { get => Get(ResidentType.Owner); set => Set(value); }
        /// <summary>
        /// Date on which the flat account is started.
        /// </summary>
        public DateTime AccountOpenedOn { get => Get<DateTime>(); set => Set(value); }
        /// <summary>
        /// Date for which the results to be calculated.
        /// </summary>
        public DateTime? DateSpecified { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(OutstandingOnSpecifiedDate)); } }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public decimal OpeningBalance { get => Get<decimal>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the expenses occured for this flat.
        /// </summary>
        public ObservableCollection<Expense> Expenses { get => Get<ObservableCollection<Expense>>(); set => Set(value); }
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
        public decimal OutstandingTillDate => GetOutstandingBalanceOnSpecifiedDate();

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
            RaiseMultiplePropertiesChanged(nameof(OutstandingTillDate), nameof(OutstandingOnSpecifiedDate));
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
            RaiseMultiplePropertiesChanged(nameof(OutstandingTillDate), nameof(OutstandingOnSpecifiedDate));
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


            decimal outstandingOnSpecifiedDate = OpeningBalance
                                                    + expensesTillSpecifiedDate
                                                    - paymentsTillSpecifiedDate;

            return outstandingOnSpecifiedDate;
        }

        #endregion
    }
}
