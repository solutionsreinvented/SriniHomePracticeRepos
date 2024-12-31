using System;

using ReInvented.ThickenerModelGenerator.Core.Interfaces;

namespace ReInvented.ThickenerModelGenerator.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public OrderService(IPaymentService paymentService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _emailService = emailService;
        }

        public void PlaceOrder(string customerEmail, decimal orderAmount)
        {
            _paymentService.ProcessPayment(orderAmount);
            _emailService.SendEmail(customerEmail, $"Order Confirmation", "Your order has been placed successfully!");
            Console.WriteLine($"Order placed successfully");
        }
    }
}
