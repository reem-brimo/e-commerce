using E_Commerce.Services.Interfaces;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Data.Models;

namespace E_Commerce.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task AddProductReservation(int ProductId, int Quantity)
        {
            var reservation = new Reservation { ProductId = ProductId, Quantity = Quantity, Expiration = DateTime.Now.AddMinutes(30) };

           await _reservationRepository.AddAsync(reservation);
        }
    }
}
