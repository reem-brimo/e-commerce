using E_Commerce.Data.Models;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using E_Commerce.SharedKernal.OperationResults;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly ICartSessionManager _sessionManager;
        private readonly IReservationService _reservationService;

        public CartService(IProductService productService,
            IReservationService reservationService,
            ICartSessionManager sessionManager)
        {
            _productService = productService;
            _reservationService = reservationService;
            _sessionManager = sessionManager;
        }


        public async Task<OperationResult<HttpStatusCode, bool>> AddToCartAsync(int productId, int quantity)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (quantity <= 0)
            {
                result.AddError("Quantity must be positive");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var product = await _productService.GetByIdAsync(productId);

            if (product.Result == null)
            {
                result.AddError("Product not available.");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            if (product.Result.Stock < quantity)
            {
                result.AddError("Insufficient stock");
                result.EnumResult = HttpStatusCode.Conflict;
                return result;
            }

            
            var cart = _sessionManager.GetCart();

            var cartProduct = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartProduct == null)
            {
                cart.Add(new CartItemDto { ProductId = productId, Quantity = quantity });
            }
            else
            {
                cartProduct.Quantity += quantity;
            }
            _sessionManager.SaveCart(cart);

            //TODO decrease inventory 
            //product.Result!.Stock -= quantity;

            //await _reservationService.AddProductReservation(productId, quantity);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;

            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> UpdateCartAsync(int productId, int quantity)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (quantity <= 0)
            {
                result.AddError("Quantity must be positive");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var cart = _sessionManager.GetCart();

            var product = await _productService.GetByIdAsync(productId);

            if (product.Result == null)
            {
                result.AddError("Product not available.");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            if (product.Result.Stock < quantity)
            {
                result.AddError("Insufficient stock");
                result.EnumResult = HttpStatusCode.Conflict;
                return result;
            }

            var cartProduct = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartProduct == null)
            {
                result.AddError("product not found in cart");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }
          
            cartProduct.Quantity = quantity;
            _sessionManager.SaveCart(cart);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public OperationResult<HttpStatusCode, bool> RemoveFromCart(int productId)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            var cart = _sessionManager.GetCart();

            cart.RemoveAll(c => c.ProductId == productId);

            _sessionManager.SaveCart(cart);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public OperationResult<HttpStatusCode, bool> ClearCart()
        {
            var result = new OperationResult<HttpStatusCode, bool>();
 
            _sessionManager.ClearCart();
            
            result.Result = true;
            result.EnumResult = HttpStatusCode.OK; 
            return result;
        }

 

        public OperationResult<HttpStatusCode, CartDto> GetCartDetails()
        {
            var result = new OperationResult<HttpStatusCode, CartDto>();
            result.Result =  new CartDto();
            result.Result.Products = new List<CartProductDto>();
            var cart = _sessionManager.GetCart();

            var productIds = cart.Select(i => i.ProductId).Distinct().ToList();

            var productsResult =  _productService.GetByIds(productIds);
            
            foreach (var item in cart)
            {
                var product = productsResult.Result.FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                {
                    result.AddError("Error in returning cart");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }


                result.Result.Products.Add(new CartProductDto
                {
                    ProductImage = product.ImageUrl,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = item.Quantity
                });

                result.Result.TotalAmount += item.Quantity * product.Price;

            }

            result.EnumResult = HttpStatusCode.OK;
            return result;
        }
    }
}
