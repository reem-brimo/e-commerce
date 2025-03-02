namespace E_Commerce.Services.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string paymentMethod);
        Task<bool> ProcessPaymentAsync(string paymentIntentId);
    }
}