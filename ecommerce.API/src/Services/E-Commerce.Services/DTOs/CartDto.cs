using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.DTOs
{   
    public class CartDto
    {
        public IList<CartProductDto> Products { get; set; }
        public double TotalAmount { get; set; }
    }
}
