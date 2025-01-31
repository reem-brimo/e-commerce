using E_Commerce.API.Controllers;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.App.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProducts()
        {
            var result = _productService.GetAll();
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{productId}")]
        [Authorize]
        public async Task<IActionResult> GetProductDetails(int productId)
        {
            var result = await _productService.GetByIdAsync(productId);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

        [HttpGet]
        [Route("/paged")]
        [Authorize]
        public IActionResult GetProductsPage(int pageNumber = 1, int pageSize = 10)
        {
            var result = _productService.GetProductsPage(pageNumber, pageSize);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDto">Product data</param>
        /// <returns>Confirmation message upon successful addition</returns>
        /// <response code="201">Product added successfully</response>
        /// <response code="403">Forbidden - Only admins can add products</response>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(ProductDetailsDto productDto)
        {
            var result =  await _productService.AddAsync(productDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="productDto">Updated product data</param>
        /// <returns>Confirmation message upon successful update</returns>
        /// <response code="200">Product updated successfully</response>
        /// <response code="403">Forbidden - Only admins can update products</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id,ProductDetailsDto productDto)
        {
            var result = await _productService.UpdateAsync(id, productDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }
        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

        [HttpGet]
        [Route("/search")]
        [Authorize]
        public ActionResult<IEnumerable<ProductDto>> SearchProducts([FromQuery] string searchTerm)
        {
            var result =  _productService.SearchProducts(searchTerm);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

    }
}
