using E_Commerce.Data.Models;
using E_Commerce.Services.Repositories;

namespace E_Commerce.Infrastructure.Repositories
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
