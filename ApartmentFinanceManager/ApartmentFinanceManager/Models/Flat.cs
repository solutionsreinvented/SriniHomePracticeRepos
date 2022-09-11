using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Store;

using System;
using System.Collections.ObjectModel;

namespace ApartmentFinanceManager.Models
{
    public class Flat : PropertyStore
    {
        private readonly ApartmentBlock _block;

        #region Constructors

        public Flat(ApartmentBlock block, string ownedBy)
            : this(block, ownedBy, DateTime.Today)
        {

        }

        public Flat(ApartmentBlock block, string ownedBy, DateTime accountOpenedOn)
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

        public string OwnedBy { get => Get<string>(); set => Set(value); }

        public string CoOwnedBy { get => Get<string>(); set => Set(value); }

        public string TenantName { get => Get<string>(); set => Set(value); }


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
    }
}
