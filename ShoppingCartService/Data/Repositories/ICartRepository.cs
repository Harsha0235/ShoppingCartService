using ShoppingCartService.Models;

namespace ShoppingCartService.Data.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetAllItemsAsync();
        Task<CartItem> GetItemByIdAsync(int id);
        Task AddItemAsync(CartItem item);
        Task UpdateItemAsync(CartItem item);
        Task DeleteItemAsync(int id);
    }
}