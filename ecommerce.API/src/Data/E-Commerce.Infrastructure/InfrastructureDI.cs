using E_Commerce.Data.Configuration;
using E_Commerce.Data.Models.Security;
using E_Commerce.Infrastructure.Factories;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Seed;
using E_Commerce.Infrastructure.Services;
using E_Commerce.Services.Interfaces;
using E_Commerce.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

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
                options.UseSqlServer(configuration.GetValue<string>("Local:ConnectionString"));
            });

            services.AddIdentity<UserSet, RoleSet>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints()
                .AddDefaultTokenProviders();
         

            await SeedData.Initialize(services);

            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IReservationRepository,ReservationRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();


            services.AddScoped<IGeminiService, GeminiService>();
            services.Configure<GeminiOptions>(configuration.GetSection(GeminiOptions.SectionName));

            services.AddHttpClient<IGeminiService, GeminiService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;

                client.BaseAddress = new Uri($"{options.BaseUrl}?key={options.ApiKey}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

            services.AddScoped<StripePaymentService>(provider =>
            {
                var stripeSettings = provider.GetRequiredService<IOptions<StripeSettings>>().Value;
                return new StripePaymentService(stripeSettings.SecretKey);
            });

            //services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));
            //services.AddScoped<PayPalPaymentService>(provider =>
            //{
            //    var payPalSettings = provider.GetRequiredService<IOptions<PayPalSettings>>().Value;
            //    return new PayPalPaymentService(payPalSettings.ClientId, payPalSettings.Secret);
            //});

            services.AddScoped<IPaymentServiceFactory, PaymentServiceFactory>();

            return services;
        }
    }
}
