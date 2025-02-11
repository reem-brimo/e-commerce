using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.InfrastructureBases;
using E_Commerce.Infrastructure.Repositories.Implementations;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using E_Commerce.SharedKernal.OperationResults;
using System.Net;

namespace E_Commerce.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OperationResult<HttpStatusCode, ProductDetailsDto>> GetByIdAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, ProductDetailsDto>();
            var productEntity = await _productRepository.GetByIdAsync(id);

            if (productEntity == null)
            {
                result.AddError("Product Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }


            var product = new ProductDetailsDto
            {
                Id = productEntity.Id,
                Description = productEntity.Description,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Stock = productEntity.Stock,
                ImageUrl = productEntity.ImageUrl,
            };

            result.Result = product;
            result.EnumResult = HttpStatusCode.OK;

            return result;

        }

        public  OperationResult<HttpStatusCode, IEnumerable<ProductDto>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<ProductDto>>();

            var products = _productRepository.GetTableNoTracking().Select(x => new ProductDto
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Price = x.Price,
            });

            result.Result = products;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> AddAsync(ProductDetailsDto productDetails)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
           
            var productEntitiy = new Product
            {
                Name = productDetails.Name,
                ImageUrl = productDetails.ImageUrl,
                Stock = productDetails.Stock,
                Description = productDetails.Description,
                Price = productDetails.Price,
            };

            var entity = await _productRepository.AddAsync(productEntitiy);

            if (entity != null)
            {
                result.EnumResult = HttpStatusCode.OK;
                result.Result = true;
            }
            else
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
           
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, ProductDetailsDto productDetails)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
           
            var productEntity = await _productRepository.GetByIdAsync(id);

            if (productEntity == null)
            {
                result.AddError("Product Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            productEntity.Name = productDetails.Name;
            productEntity.ImageUrl = productDetails.ImageUrl;
            productEntity.Stock = productDetails.Stock;
            productEntity.Description = productDetails.Description;

            await _productRepository.UpdateAsync(productEntity);


            result.EnumResult = HttpStatusCode.OK;
            result.Result = true;
            
            return result;

        }

        public async Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            var productEntity = await _productRepository.GetByIdAsync(id);


            if (productEntity == null)
            {
                result.AddError("Product Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                result.Result = false;
                return result;
            }

            await _productRepository.DeleteAsync(productEntity);

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;

            return result;

        }

        public OperationResult<HttpStatusCode, IEnumerable<ProductDto>> GetProductsPage(int pageNumber = 1, int pageSize = 10)
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<ProductDto>>();

            var products = _productRepository.GetTableNoTracking()
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Name = x.Name,
                    Price = x.Price
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            result.Result = products;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public OperationResult<HttpStatusCode, IEnumerable<ProductDto>> SearchProducts(string searchTerm)
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<ProductDto>>();

            var products = _productRepository.GetTableNoTracking()
                .Where(p => p.Name.Contains(searchTerm))
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Id = p.Id,
                    Price = p.Price
                })
                .ToList();

            result.EnumResult = HttpStatusCode.OK;
            result.Result = products;

            return result;
        }


    }



}
