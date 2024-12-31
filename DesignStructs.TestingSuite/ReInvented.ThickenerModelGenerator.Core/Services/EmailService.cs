using System;

using ReInvented.ThickenerModelGenerator.Core.Interfaces;

namespace ReInvented.ThickenerModelGenerator.Core.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Email sent to {to}: {subject}\n{body}");
        }
    }
}
