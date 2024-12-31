using System;

using ReInvented.ThickenerModelGenerator.Core.Interfaces;

namespace ReInvented.ThickenerModelGenerator.Core.Services
{
    public class CreditCardPaymentService : IPaymentService
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processed credit card payment of {amount:C}");
        }
    }
}
