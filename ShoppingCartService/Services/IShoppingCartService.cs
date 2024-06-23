using ShoppingCartService.Models;

namespace ShoppingCartService.Services
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync();
        Task<CartItem> GetCartItemByIdAsync(int id);
        Task AddItemAsync(CartItem item);
        Task UpdateItemAsync(int id, CartItem item);
        Task DeleteItemAsync(int id);
    }
}