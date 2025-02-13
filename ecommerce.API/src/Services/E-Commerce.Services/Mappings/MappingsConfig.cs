using E_Commerce.Data.Models;
using E_Commerce.Services.DTOs;
using Mapster;

namespace Mosque.Services.Mappings
{
    public static class MappingsConfig
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Product, ProductDetailsDto>.NewConfig();
            TypeAdapterConfig<ProductDetailsDto, Product>.NewConfig();


            TypeAdapterConfig<UserOrderDto, Order>.NewConfig();
            TypeAdapterConfig<OrderItemDto, OrderItem>.NewConfig();
     

        }
    }
}
