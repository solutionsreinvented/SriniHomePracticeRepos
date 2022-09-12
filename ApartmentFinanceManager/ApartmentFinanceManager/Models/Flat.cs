using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Stores;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApartmentFinanceManager.Models
{
    public class Flat : PropertyStore
    {
        private readonly ApartmentBlock _block;

        #region Constructors

        private Flat()
        {
            DateSpecified = DateTime.Today;
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

        /// <summary>
        /// Flat number.
        /// </summary>
        public string Number { get => Get<string>(); set => Set(value); }
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
        /// Provides the complete description of the flat.
        /// </summary>
        public string Description => $"{_block?.Name}{Number}";
        /// <summary>
        /// Date on which the flat account is started.
        /// </summary>
        public DateTime AccountOpenedOn { get => Get<DateTime>(); set => Set(value); }
        /// <summary>
        /// Date for which the results to be calculated.
        /// </summary>
        public DateTime? DateSpecified { get => Get<DateTime>(); set => Set(value); }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public decimal OpeningBalance { get => Get<decimal>(); set => Set(value); }
        /// <summary>
        /// Outstanding balance pending as on the selected date.
        /// </summary>
        public decimal OutstandingOnSpecifiedDate => GetOutstandingBalanceOnSpecifiedDate(DateSpecified);
        /// <summary>
        /// Outstanding balance pending calculated till date.
        /// </summary>
        public decimal OutstandingTillDate => GetOutstandingBalanceOnSpecifiedDate();
        /// <summary>
        /// Keeps track of the all the expenses occured for this flat.
        /// </summary>
        public ObservableCollection<Expense> Expenses { get => Get<ObservableCollection<Expense>>(); set => Set(value); }
        /// <summary>
        /// Keeps track of the all the payments made by the flat owner.
        /// </summary>
        public ObservableCollection<Payment> Payments { get => Get<ObservableCollection<Payment>>(); set => Set(value); }

        #region Helper Methods

        private decimal GetOutstandingBalanceOnSpecifiedDate(DateTime? calculateTill = null)
        {
            if (DateSpecified == null)
            {
                DateSpecified = DateTime.Today;
            }

            decimal expensesTillSpecifiedDate = Expenses == null ? 0.0m : Expenses.Where(e => e.OccuredOn <= DateSpecified).Sum(e => e.Amount);

            decimal paymentsTillSpecifiedDate = Payments == null ? 0.0m : Payments.Where(p => p.ReceivedOn <= DateSpecified).Sum(p => p.Amount);


            decimal outstandingOnSpecifiedDate = OpeningBalance
                                                    + expensesTillSpecifiedDate
                                                    - paymentsTillSpecifiedDate;

            return outstandingOnSpecifiedDate;
        } 

        #endregion
    }
}
