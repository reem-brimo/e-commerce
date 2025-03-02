using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services.UseCases
{
    public class CreatePaymentUseCase
    {
        private readonly IPaymentServiceFactory _paymentServiceFactory;

        public CreatePaymentUseCase(IPaymentServiceFactory paymentServiceFactory)
        {
            _paymentServiceFactory = paymentServiceFactory;
        }

        public async Task<PaymentResult> Execute(decimal amount, string currency, string description, string customerEmail, string token, string paymentMethod)
        {
            // Resolve the correct payment service
            var paymentService = _paymentServiceFactory.GetPaymentService(paymentMethod);

            // Process the payment
            return await paymentService.ProcessPaymentAsync(amount, currency, description, customerEmail, token);
        }
    }
}
