using E_Commerce.API.Controllers;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.App.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class CartController(ICartService cartService) : BaseController
    {
        private readonly ICartService _cartService = cartService;


        [HttpGet]
        public IActionResult GetCart()
        {
            var result = _cartService.GetCartDetails();
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        [HttpPost]
        [Route("{productId}/{quantity}")]
        
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {

            var result = await _cartService.AddToCartAsync(productId, quantity);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

       
        [HttpPut]
        [Route("{productId}/{quantity}")]
        //[Authorize]
        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            var result1 = await _cartService.UpdateCartAsync(productId, quantity);
            return GetResult(result1.ErrorMessages, result1.EnumResult, result1.Result);

        }

        [HttpDelete]
        [Route("{productId}")]

        public IActionResult RemoveFromCart(int productId)
        {
            var result = _cartService.RemoveFromCart(productId);
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
