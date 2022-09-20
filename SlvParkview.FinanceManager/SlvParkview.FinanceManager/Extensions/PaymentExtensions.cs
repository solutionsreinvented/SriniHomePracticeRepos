﻿using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Extensions
{
    public static class PaymentExtensions
    {
        public static PaymentInfo ParseToPaymentInfo(this Payment payment, string flatNumber)
        {
            return new PaymentInfo()
            {
                Amount = payment.Amount.FormatNumber("N1"),
                FlatNumber = flatNumber,
                Mode = payment.Mode.ToString(),
                ReceivedOn = payment.ReceivedOn.ToString("dd-MM-yyyy"),
                Category = payment.Category.ToString()
            };
        }
    }
}
