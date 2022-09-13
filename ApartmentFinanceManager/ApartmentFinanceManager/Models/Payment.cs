using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Stores;

using System;

namespace ApartmentFinanceManager.Models
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

        public PaymentMode Mode { get => Get(PaymentMode.Cash); set => Set(value); }
        /// <summary>
        /// Date on which the payment is made.
        /// </summary>
        public DateTime ReceivedOn { get => Get(DateTime.Today); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the payment.
        /// </summary>
        public TransactionCategory Category { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }

        public bool IsDataValid => Amount > 0.0m && ReceivedOn <= DateTime.Today;

        #region Equality

        public override int GetHashCode()
        {
            return $"{ReceivedOn}{Category}{Amount}".GetHashCode();
        }

        public bool Equals(Payment payment)
        {
            return payment != null &&
                   ReceivedOn == payment.ReceivedOn && Category == payment.Category && Amount == payment.Amount;
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

    }
}