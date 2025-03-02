using E_Commerce.Data.Models;
using E_Commerce.Services.Implementation;
using E_Commerce.Services.Repositories;
using Moq;
using System.Net;

namespace Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            // Arrange: Initialize mock repository and service
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public async void GetProductById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Test Product", Price = 100.0 };

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Result.Id);
            Assert.Equal(expectedProduct.Name, result.Result.Name);
            Assert.Equal(expectedProduct.Price, result.Result.Price);
        }

        [Fact]
        public async Task GetProductById_ThrowsKeyNotFoundException_WhenProductDoesNotExistAsync()
        {
            // Arrange
            var productId = 999; // Non-existent product ID

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act & Assert
            var result = await  _productService.GetByIdAsync(productId);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Null(result.Result);
            Assert.Contains("Product Not Found", result.ErrorMessages);
        }
    }

}
