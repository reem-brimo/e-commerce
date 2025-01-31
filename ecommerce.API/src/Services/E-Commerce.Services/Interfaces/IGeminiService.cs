
using System.Net;
using E_Commerce.SharedKernal.OperationResults;

namespace E_Commerce.Services.Interfaces
{
    public interface IGeminiService
    {
        
        Task<OperationResult<HttpStatusCode, string>> ChatAsync(string message);
    }
}
