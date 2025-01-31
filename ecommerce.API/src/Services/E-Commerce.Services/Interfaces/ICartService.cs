using E_Commerce.SharedKernal.OperationResults;
using System.Net;

namespace E_Commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<OperationResult<HttpStatusCode, bool>> AddToCart(int productId, int quantity);
        OperationResult<HttpStatusCode, bool> UpdateCart(int productId, int quantity);
        OperationResult<HttpStatusCode, bool> RemoveFromCart(int productId);
        OperationResult<HttpStatusCode, bool> ClearCart();
    }
}
