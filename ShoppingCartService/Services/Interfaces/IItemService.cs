using ShoppingCartService.Models;

namespace ShoppingCartService.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task<Item> AddItemAsync(Item item);
        Task<Item> UpdateItemAsync(int id, Item item);
        Task DeleteItemAsync(int id);
    }
}