namespace E_Commerce.Services.DTOs
{
    public class CartProductDto
    {
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
