namespace E_Commerce.Services.DTOs
{
    public class UserOrderDto
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        
        public string PostCode { get; set; }
        public string City { get; set; }

        public List<OrderItemDto> items { get; set; }
        //public string StripeReference { get; set; }
        //public string SessionId { get; set; }
        //public List<Stock> Stocks { get; set; }
    }
}
