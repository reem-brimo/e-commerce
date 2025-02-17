using E_Commerce.Data.Models;
using E_Commerce.Data.Models.Security;

namespace E_Commerce.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Address { get; set; }

        public UserSet User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
