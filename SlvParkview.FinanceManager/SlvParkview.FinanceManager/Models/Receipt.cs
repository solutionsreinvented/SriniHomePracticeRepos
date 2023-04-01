using Newtonsoft.Json;

using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Enums;

using System;
using System.Xml.Serialization;

namespace SlvParkview.FinanceManager.Models
{
    public class Receipt : PropertyStore
    {
        #region Default Constructor

        public Receipt()
        {
            Mode = ReceiptMode.AppPayment;
        }

        #endregion

        #region Parameterized Constructors

        public Receipt(TransactionCategory category, decimal amount)
            : this(category, amount, DateTime.Today)
        {

        }

        public Receipt(TransactionCategory category, decimal amount, DateTime receivedOn)
            : this()
        {
            Category = category;
            Amount = amount;
            ReceivedOn = receivedOn;
        }

        #endregion

        #region Public Properties

        public ReceiptMode Mode
        {
            get => Get<ReceiptMode>();
            set
            {
                Set(value);
                EnableReferenceId = value != ReceiptMode.Cash;
            }
        }
        /// <summary>
        /// Date on which the receipt is made.
        /// </summary>
        public DateTime ReceivedOn { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }
        /// <summary>
        /// Date on which the receipt is entered in to the database.
        /// </summary>
        public DateTime RecordedOn { get => Get(ReceivedOn); set => Set(value); }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the receipt.
        /// </summary>
        public TransactionCategory Category { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }
        /// <summary>
        /// Reference ID of the transaction. Applicable for online transfer only.
        /// </summary>
        public string ReferenceId { get => Get<string>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        [JsonIgnore]
        [XmlIgnore]
        public bool IsDataValid => Amount > 0.0m && ReceivedOn <= DateTime.Today;

        [JsonIgnore]
        [XmlIgnore]
        public bool EnableReferenceId { get => Get<bool>(); private set => Set(value); }
        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return base.GetHashCode();
            ///return $"{ReceivedOn}{Category}{Amount}".GetHashCode();
        }

        public bool Equals(Receipt receipt)
        {
            return receipt != null &&
                   ReceivedOn == receipt.ReceivedOn && Category == receipt.Category &&
                   Amount == receipt.Amount && Mode == receipt.Mode;
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is Receipt receipt && Equals(receipt);
        }

        public static bool operator ==(Receipt a, Receipt b)
        {
            return ReferenceEquals(a, b) || (!(a is null) && !(b is null) && a.Equals(b));
        }

        public static bool operator !=(Receipt a, Receipt b)
        {
            return !(a == b);
        }

        #endregion

        #region Prototypes

        public static Receipt Clone(Receipt receipt)
        {
            return new Receipt()
            {
                ReceivedOn = receipt.ReceivedOn,
                Amount = receipt.Amount,
                Category = receipt.Category,
                Mode = receipt.Mode
            };
        }
        #endregion
    }
}