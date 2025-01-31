using E_Commerce.Data.Models.BaseModels;

namespace E_Commerce.Data.Models
{
    public class Reservation : BaseEntity
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime Expiration { get; set; }

        public Product Product { get; set; }
    }
}
