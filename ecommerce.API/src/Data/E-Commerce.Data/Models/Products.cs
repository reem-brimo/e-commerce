namespace E_Commerce.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Stock { get; set; }
        public string? ImageUrl { get; set; }


        public ICollection<OrderItem> OrderItem { get; set; }

    }
}
