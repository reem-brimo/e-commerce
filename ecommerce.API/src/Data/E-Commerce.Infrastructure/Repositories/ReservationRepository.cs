using E_Commerce.Data.Models;
using E_Commerce.Services.Repositories;

namespace E_Commerce.Infrastructure.Repositories
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
