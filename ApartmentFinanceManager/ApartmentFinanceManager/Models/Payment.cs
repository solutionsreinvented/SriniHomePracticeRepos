using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Store;

using System;

namespace ApartmentFinanceManager.Models
{
    public class Payment : PropertyStore
    {
        public Payment()
        {
            Mode = PaymentMode.Cash;
        }

        public Payment(PaymentCategory paymentCategory, decimal amount)
            : this(paymentCategory, amount, DateOnly.FromDateTime(DateTime.Today))
        {

        }

        public Payment(PaymentCategory paymentCategory, decimal amount, DateOnly occuredOn)
            : this()
        {
            PaymentCategory = paymentCategory;
            Amount = amount;
            ReceivedOn = occuredOn;
        }
        public PaymentMode Mode { get => Get<PaymentMode>(); set => Set(value); }
        /// <summary>
        /// Date(only) on which the payment is made.
        /// </summary>
        public DateOnly ReceivedOn { get => Get<DateOnly>(); set => Set(value); }
        /// <summary>
        /// Category <see cref="Enums.PaymentCategory"/> of the payment.
        /// </summary>
        public PaymentCategory PaymentCategory { get => Get(PaymentCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set => Set(value); }

    }
}