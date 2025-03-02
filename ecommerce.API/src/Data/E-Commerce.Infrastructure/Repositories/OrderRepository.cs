using E_Commerce.Data.Models;
using E_Commerce.Services.Repositories;

namespace E_Commerce.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Order>(dbContext), IOrderRepository
    {
        private readonly ApplicationDbContext _context = dbContext;
    }
}
