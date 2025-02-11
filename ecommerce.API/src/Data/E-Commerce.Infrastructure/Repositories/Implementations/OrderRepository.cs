using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.InfrastructureBases;
using E_Commerce.Infrastructure.Repositories.Interfaces;

namespace E_Commerce.Infrastructure.Repositories.Implementations
{
    public class OrderRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Order>(dbContext), IOrderRepository
    {
        private readonly ApplicationDbContext _context = dbContext;
    }
}
