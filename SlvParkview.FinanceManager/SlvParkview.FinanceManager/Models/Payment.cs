using Newtonsoft.Json;
using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Enums;

using System;
using System.Xml.Serialization;

namespace SlvParkview.FinanceManager.Models
{
    public class Payment : PropertyStore
    {
        #region Default Constructor

        public Payment()
        {

        }

        #endregion

        #region Parameterized Constructors

        public Payment(TransactionCategory category, decimal amount)
            : this(category, amount, DateTime.Today)
        {

        }

        public Payment(TransactionCategory category, decimal amount, DateTime receivedOn)
            : this()
        {
            Category = category;
            Amount = amount;
            ReceivedOn = receivedOn;
        }

        #endregion

        #region Public Properties

        public PaymentMode Mode { get => Get(PaymentMode.Cash); set => Set(value); }
        /// <summary>
        /// Date on which the payment is made.
        /// </summary>
        public DateTime ReceivedOn { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }
        /// <summary>
        /// Date on which the payment is entered in to the database.
        /// </summary>
        public DateTime RecordedOn { get => Get(ReceivedOn); set { Set(value); } }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the payment.
        /// </summary>
        public TransactionCategory Category { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }

        //public string ReferenceId { get => Get<string>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        [JsonIgnore]
        [XmlIgnore]
        public bool IsDataValid => Amount > 0.0m && ReceivedOn <= DateTime.Today;

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return base.GetHashCode();
            ///return $"{ReceivedOn}{Category}{Amount}".GetHashCode();
        }

        public bool Equals(Payment payment)
        {
            return payment != null &&
                   ReceivedOn == payment.ReceivedOn && Category == payment.Category &&
                   Amount == payment.Amount && Mode == payment.Mode;
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is Payment payment && Equals(payment);
        }

        public static bool operator ==(Payment a, Payment b)
        {
            return ReferenceEquals(a, b) || (!(a is null) && !(b is null) && a.Equals(b));
        }

        public static bool operator !=(Payment a, Payment b)
        {
            return !(a == b);
        }

        #endregion

        #region Prototypes

        public static Payment Clone(Payment payment)
        {
            return new Payment()
            {
                ReceivedOn = payment.ReceivedOn,
                Amount = payment.Amount,
                Category = payment.Category,
                Mode = payment.Mode
            };
        }
        #endregion
    }
}