using System;
using System.Xml.Serialization;

using Newtonsoft.Json;

using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Enums;

namespace SlvParkview.FinanceManager.Models
{
    public class Bill : PropertyStore
    {
        #region Default Constructor

        public Bill()
        {

        }

        #endregion

        #region Paramterized Constructors

        public Bill(TransactionCategory category, decimal amount)
            : this(category, amount, DateTime.Today)
        {

        }
        public Bill(TransactionCategory category, decimal amount, DateTime occuredOn)
        {
            Category = category;
            Amount = amount;
            OccuredOn = occuredOn;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Date on which the expenditure is recorded.
        /// </summary>
        public DateTime OccuredOn { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the bill.
        /// </summary>
        public TransactionCategory Category { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }

        [JsonIgnore]
        [XmlIgnore]
        public bool IsDataValid => OccuredOn <= DateTime.Today && Amount > 0.0m;

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return base.GetHashCode();
            ///return $"{OccuredOn}{Category}{Amount}".GetHashCode();
        }

        public bool Equals(Bill bill)
        {
            return bill != null &&
                   OccuredOn == bill.OccuredOn && Category == bill.Category && Amount == bill.Amount;
        }


        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is Bill bill && Equals(bill);
        }

        public static bool operator ==(Bill a, Bill b)
        {
            return ReferenceEquals(a, b) || !(a is null) && !(b is null) && a.Equals(b);
        }

        public static bool operator !=(Bill a, Bill b)
        {
            return !(a == b);
        }

        #endregion

        #region Prototypes

        public static Bill Clone(Bill bill)
        {
            return new Bill()
            {
                OccuredOn = bill.OccuredOn,
                Amount = bill.Amount,
                Category = bill.Category,
            };
        }

        #endregion

    }
}