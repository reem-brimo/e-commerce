using E_Commerce.API.Controllers;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.App.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

       
        [HttpPost]
        [Route("{productId}/{quantity}")]
        
        public async Task<IActionResult> AddToCartAsync(int productId, int quantity)
        {

            var result = await _cartService.AddToCart(productId, quantity);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

       
        [HttpPut]
        [Route("{productId}/{quantity}")]
        [Authorize]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var result1 = _cartService.UpdateCart(productId, quantity);
            return GetResult(result1.ErrorMessages, result1.EnumResult, result1.Result);

        }

        [HttpDelete]
        [Route("{productId}")]

        public IActionResult RemoveFromCart(int id)
        {
            var result = _cartService.RemoveFromCart(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

        [HttpDelete]
        public IActionResult ClearCart()
        {
            var result = _cartService.ClearCart();
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

    }
}
