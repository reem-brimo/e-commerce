using E_Commerce.Services.DTOs;
using E_Commerce.SharedKernal.OperationResults;
using System.Net;

namespace E_Commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<OperationResult<HttpStatusCode, bool>> AddToCartAsync(int productId, int quantity);
        Task<OperationResult<HttpStatusCode, bool>> UpdateCartAsync(int productId, int quantity);
        OperationResult<HttpStatusCode, bool> RemoveFromCart(int productId);
        OperationResult<HttpStatusCode, bool> ClearCart();

        OperationResult<HttpStatusCode, CartDto> GetCartDetails();

    }
}
