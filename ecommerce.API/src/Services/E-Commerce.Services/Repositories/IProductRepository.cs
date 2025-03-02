using E_Commerce.Data.Models;
using E_Commerce.Services.InfrastructureBases;

namespace E_Commerce.Services.Repositories
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        IEnumerable<Product> GetByIds(List<int> ids);
      
    }
}
