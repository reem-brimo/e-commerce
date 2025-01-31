namespace E_Commerce.Services.DTOs
{
    public class ProductDetailsDto : ProductDto
    {
        public string Description { get; set; } = string.Empty;

        public int Stock { get; set; }
    }
}
