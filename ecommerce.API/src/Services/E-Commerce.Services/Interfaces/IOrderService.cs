using E_Commerce.Services.DTOs;
using E_Commerce.SharedKernal.OperationResults;
using System.Net;

namespace E_Commerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OperationResult<HttpStatusCode, bool>> AddAsync(UserOrderDto Order);
    }
}

