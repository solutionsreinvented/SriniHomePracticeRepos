namespace ReInvented.ThickenerModelGenerator.Core.Interfaces
{
    public interface IPaymentService
    {
        void ProcessPayment(decimal amount);
    }
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
    public interface IOrderService
    {
        void PlaceOrder(string customerEmail, decimal orderAmount);
    }
}
