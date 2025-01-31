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
    }


}
