using System;

using ReInvented.ThickenerModelGenerator.Core.Interfaces;

namespace ReInvented.ThickenerModelGenerator.Core.Services
{
    public class PayPalPaymentService : IPaymentService
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processed PayPal payment of {amount:C}");
        }
    }
}
