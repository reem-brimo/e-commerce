using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace E_Commerce.Services.Implementation
{
    public class CartSessionManager : ICartSessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartSessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItemDto> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var cartData = session?.Get(CartSessionKey);

            return cartData == null
                ? new List<CartItemDto>()
                : JsonSerializer.Deserialize<List<CartItemDto>>(cartData)!;
        }

        public void SaveCart(List<CartItemDto> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Set(CartSessionKey, JsonSerializer.SerializeToUtf8Bytes(cart));
        }

        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Remove(CartSessionKey);
        }
    }
}
