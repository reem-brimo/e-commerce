using E_Commerce.Data.Models.Security;
using E_Commerce.Infrastructure.Repositories.Implementations;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureDI
    {
        public static async Task<IServiceCollection> AddInfrastructureDependenciesAsync(
              this IServiceCollection services,
        IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("Server:ConnectionString"));

            });

            services.AddIdentity<UserSet, RoleSet>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints()
                .AddDefaultTokenProviders();
         

            await SeedData.Initialize(services);

            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IReservationRepository,ReservationRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
       

            return services;
        }
    }
}
