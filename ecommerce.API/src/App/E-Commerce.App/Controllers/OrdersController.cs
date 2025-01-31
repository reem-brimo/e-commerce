using E_Commerce.API.Controllers;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.App.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IProductService _productService;

        public OrdersController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>List of products</returns>
        // [HttpGet]
        // //[Authorize(Roles = "Admin")]
        // public IActionResult CreateOrder()
        // {
        //     var result = _productService.GetAll();
        //     return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        // }

        // /// <summary>
        // /// Retrieves a product by ID.
        // /// </summary>
        // /// <param name="id">Product ID</param>
        // /// <returns>Product details</returns>
        // [HttpGet]
        // [Route("[action]")]
        // [Authorize]
        // public async Task<IActionResult> CheckOut(int id)
        // {
        //     var result = await _productService.GetByIdAsync(id);
        //     return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        // }


    }
}
