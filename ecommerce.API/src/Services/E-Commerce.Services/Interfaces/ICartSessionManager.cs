using E_Commerce.Services.DTOs;

namespace E_Commerce.Services.Interfaces
{
    public interface ICartSessionManager
    {
        List<CartItemDto> GetCart();
        void SaveCart(List<CartItemDto> cart);
        void ClearCart();
    }
}
