using E_Commerce.Infrastructure.InfrastructureBases;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Data.Models;

namespace E_Commerce.Infrastructure.Repositories.Implementations
{
    public class ReservationRepository : GenericRepositoryAsync<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
