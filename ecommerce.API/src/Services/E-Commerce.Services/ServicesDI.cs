using E_Commerce.Services.Configuration;
using E_Commerce.Services.Implementation;
using E_Commerce.Services.Implementation4;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mosque.Services.Mappings;
using System.Net.Http.Headers;
using System.Text;

namespace E_Commerce.Services
{
    public static class ServicesDI
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            MappingsConfig.ConfigureMappings();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartSessionManager, CartSessionManager>();


            services.AddScoped<IGeminiService, GeminiService>();
            services.Configure<GeminiOptions>(configuration.GetSection(GeminiOptions.SectionName));

            services.AddHttpClient<IGeminiService, GeminiService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;

                client.BaseAddress = new Uri($"{options.BaseUrl}?key={options.ApiKey}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            return services;
        }
    }
}


