namespace E_Commerce.Services.Interfaces
{
    public interface IPaymentServiceFactory
    {
        IPaymentService GetPaymentService(string paymentMethod);
    }
}
