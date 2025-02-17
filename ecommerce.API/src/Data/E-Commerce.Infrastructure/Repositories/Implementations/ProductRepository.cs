using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.InfrastructureBases;
using E_Commerce.Infrastructure.Repositories.Interfaces;

namespace E_Commerce.Infrastructure.Repositories.Implementations
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetByIds(List<int> ids)
        {
            return _context.Products.Where(p => ids.Contains(p.Id));                   
        }
    }


}
