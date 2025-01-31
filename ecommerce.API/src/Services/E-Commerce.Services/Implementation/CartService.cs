using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using E_Commerce.SharedKernal.OperationResults;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private readonly IReservationService _reservationService;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor, IProductService productService, IReservationService reservationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _reservationService = reservationService;
        }


        public async Task<OperationResult<HttpStatusCode, bool>> AddToCart(int productId, int quantity)
        {
            var result = new OperationResult<HttpStatusCode, bool>();


            var product = await _productService.GetByIdAsync(productId);

            if (product.Result == null || product.Result.Stock < quantity)
            {
                result.AddError("Product not available.");
                result.EnumResult = HttpStatusCode.NotFound;
            }

           
            var cart = GetCart();
            var cartProduct = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartProduct == null)
            {
                cart.Add(new CartItemDto { ProductId = productId, Quantity = quantity });
            }
            else
            {
                cartProduct.Quantity += quantity;
            }
            SaveCart(cart);

            //decrease inventory 
            //product.Result!.Stock -= quantity;

            //await _reservationService.AddProductReservation(productId, quantity);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;

            return result;
        }

        public OperationResult<HttpStatusCode, bool> UpdateCart(int productId, int quantity)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
                result.Result = true;
                result.EnumResult = HttpStatusCode.OK;
            }

            else
            {
                result.AddError("item not found in cart");
                result.EnumResult = HttpStatusCode.NotFound;
            }

            return result;
        }

        public OperationResult<HttpStatusCode, bool> RemoveFromCart(int productId)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            var cart = GetCart();
            cart.RemoveAll(c => c.ProductId == productId);
            SaveCart(cart);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public OperationResult<HttpStatusCode, bool> ClearCart()
        {
            var result = new OperationResult<HttpStatusCode, bool>();
 
            SaveCart(new List<CartItemDto>());
            
            result.Result = true;
            result.EnumResult = HttpStatusCode.OK; 
            return result;
        }


        private void SaveCart(List<CartItemDto> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Set(CartSessionKey, JsonSerializer.SerializeToUtf8Bytes(cart));
        }

        private List<CartItemDto> GetCart()
        {
            byte[]? cart = null;
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.TryGetValue(CartSessionKey, out cart);
            if (cart != null)
                return JsonSerializer.Deserialize<List<CartItemDto>>(cart) ?? new List<CartItemDto>();
            return new List<CartItemDto>();
        }
    }
}
