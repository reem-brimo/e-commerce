namespace E_Commerce.Services.Interfaces
{
    public interface IReservationService
    {
        Task AddProductReservation(int ProductId, int Quantity);
    }
}
