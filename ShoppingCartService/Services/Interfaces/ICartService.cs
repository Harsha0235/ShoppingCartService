using ShoppingCartService.Models;

namespace ShoppingCartService.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task<CartItem> AddItemToCartAsync(string userId, int itemId, int quantity);
        Task<CartItem> UpdateCartItemAsync(string userId, int itemId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int itemId);
    }
}