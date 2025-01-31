using E_Commerce.SharedKernal.OperationResults;
using E_Commerce.Services.DTOs;
using System.Net;

namespace E_Commerce.Services.Interfaces
{
    public interface IProductService 
    {
        Task<OperationResult<HttpStatusCode, ProductDetailsDto>> GetByIdAsync(int id);
        OperationResult<HttpStatusCode, IEnumerable<ProductDto>> GetAll();
        Task<OperationResult<HttpStatusCode, bool>> AddAsync(ProductDetailsDto product);
        Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, ProductDetailsDto product);
        Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id);

        OperationResult<HttpStatusCode, IEnumerable<ProductDto>> GetProductsPage(int pageNumber = 1, int pageSize = 10);
        OperationResult<HttpStatusCode, IEnumerable<ProductDto>> SearchProducts(string searchTerm);
    }
}
