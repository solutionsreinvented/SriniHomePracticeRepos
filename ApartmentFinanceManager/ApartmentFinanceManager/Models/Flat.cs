using ApartmentFinanceManager.Enums;
using ReInvented.Shared.Store;
using System;
using System.Collections.ObjectModel;

namespace ApartmentFinanceManager.Models
{
    public class Flat : PropertyStore
    {
        private readonly Block _block;

        public Flat(Block block, string ownedBy)
        {
            _block = block;
            OwnedBy = ownedBy;
        }
        /// <summary>
        /// Flat number.
        /// </summary>
        public int Number { get; set; }

        public string OwnedBy { get; set; }

        public ResidentType ResidentType { get; set; }

        /// <summary>
        /// Provides the complete description of the flat.
        /// </summary>
        public string Description => $"{_block?.Name}{Number}";
        /// <summary>
        /// Date on which the flat account is started.
        /// </summary>
        public DateTime AccountOpenedOn { get; set; }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public decimal OpeningBalance { get; set; }
        /// <summary>
        /// Keeps track of the all the expenses occured for this flat.
        /// </summary>
        public ObservableCollection<Expense> Expenses { get; set; }
        /// <summary>
        /// Keeps track of the all the payments made by the flat owner.
        /// </summary>
        public ObservableCollection<Payment> Payments { get; set; }
    }
}
