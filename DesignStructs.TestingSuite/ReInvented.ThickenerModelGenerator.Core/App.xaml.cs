using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using ReInvented.ThickenerModelGenerator.Core.Interfaces;
using ReInvented.ThickenerModelGenerator.Core.Services;

namespace ReInvented.ThickenerModelGenerator.Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceCollection services = new ServiceCollection();

            _ = services.AddTransient<CreditCardPaymentService>()
                        .AddTransient<PayPalPaymentService>()
                        .AddTransient<IEmailService, EmailService>()
                        .AddTransient<IOrderService, OrderService>()
                        .AddTransient(provider =>
                        {
                            bool useCreditCard = false;
                            IPaymentService service = useCreditCard ? (IPaymentService)provider.GetService<CreditCardPaymentService>() : provider.GetService<PayPalPaymentService>();
                            return service;
                        });

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IOrderService orderService = serviceProvider.GetService<IOrderService>();
            orderService?.PlaceOrder("sriinivas.masanam@gmail.com", 25.89m);
        }
    }
}
