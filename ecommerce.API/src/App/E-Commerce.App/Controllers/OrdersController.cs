using E_Commerce.API.Controllers;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.App.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService) : BaseController
    {
        private readonly IOrderService _orderService = orderService;


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync(UserOrderDto order)
        {
            var result = await _orderService.AddAsync(order);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        //get old orders
        //[HttpGet]
        //[Route("/user-orders")]
        //[Authorize]
        //public async Task<IActionResult>GetOrdersAsync()
        //{
        //    var result = await _productService.GetByIdAsync(id);
        //    return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        //}


    }
}
