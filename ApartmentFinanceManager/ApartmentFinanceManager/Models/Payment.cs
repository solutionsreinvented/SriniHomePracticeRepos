using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Stores;

using System;

namespace ApartmentFinanceManager.Models
{
    public class Payment : PropertyStore
    {
        public Payment()
        {
            ///Mode = PaymentMode.Cash;
        }

        public Payment(TransactionCategory paymentCategory, decimal amount)
            : this(paymentCategory, amount, DateTime.Today)
        {

        }

        public Payment(TransactionCategory paymentCategory, decimal amount, DateTime receivedOn)
            : this()
        {
            Category = paymentCategory;
            Amount = amount;
            ReceivedOn = receivedOn;
        }
        public PaymentMode Mode { get => Get(PaymentMode.Cash); set => Set(value); }
        /// <summary>
        /// Date on which the payment is made.
        /// </summary>
        public DateTime ReceivedOn { get => Get(DateTime.Today); set => Set(value); }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the payment.
        /// </summary>
        public TransactionCategory Category { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set => Set(value); }

    }
}