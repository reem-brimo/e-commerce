using E_Commerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace E_Commerce.Services.BackgroundServices
{
    public class ReservationCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceFactory;
        public ReservationCleanupService(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                
                using var scope = _serviceFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var now = DateTime.UtcNow;

                // Find expired reservations
                var expiredReservations = await dbContext.Reservations
                    .Where(r => r.Expiration <= now)
                    .ToListAsync(stoppingToken);

                if (expiredReservations.Any())
                {

                    foreach (var reservation in expiredReservations)
                    {
                        var product = await dbContext.Products
                            .FirstOrDefaultAsync(p => p.Id == reservation.ProductId, stoppingToken);

                        if (product != null)
                        {
                            product.Stock += reservation.Quantity;
                            dbContext.Products.Update(product);
                        }

                        dbContext.Reservations.Remove(reservation);
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
