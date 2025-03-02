
using E_Commerce.Infrastructure.Services;
using E_Commerce.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Factories
{
    public class PaymentServiceFactory : IPaymentServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentService GetPaymentService(string paymentMethod)
        {
            return paymentMethod switch
            {
                "Stripe" => _serviceProvider.GetRequiredService<StripePaymentService>(),
                //"PayPal" => _serviceProvider.GetRequiredService<PayPalPaymentService>(),
                _ => throw new ArgumentException("Invalid payment method", nameof(paymentMethod))
            };
        }
    }
}
