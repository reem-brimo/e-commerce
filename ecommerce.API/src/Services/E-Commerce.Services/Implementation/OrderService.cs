using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using E_Commerce.SharedKernal.OperationResults;
using System.Net;

namespace E_Commerce.Services.Implementation
{
    public class OrderService(IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserService userService,
        ICartService cartService) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUserService _userService = userService;
        private readonly ICartService _cartService = cartService;

        public async Task<OperationResult<HttpStatusCode, bool>> AddAsync(UserOrderDto order)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            double totalPrice = 0;

            foreach (var item in order.items)
            {

                var productResult = _productRepository.GetByIdAsync(item.ProductId);
                if (productResult.Result != null)
                {
                    totalPrice += item.Quantity * productResult.Result.Price;
                }
                else
                {
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.AddError("Failed to place order, product not found");
                    return result;
                }
            }

            var orderEntitiy = new Order
            {
                UserId = await _userService.GetUserId(),
                Address = order.Address,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice,
                OrderItems = order.items.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,

                }).ToList(),
            };

            var entity = await _orderRepository.AddAsync(orderEntitiy);

            if (entity != null)
            {
                _cartService.ClearCart();

                result.EnumResult = HttpStatusCode.OK;
                result.Result = true;
            }
            else
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
            }

            return result;

        }
    }
}
