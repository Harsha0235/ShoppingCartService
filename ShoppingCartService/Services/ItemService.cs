using ShoppingCartService.Models;
using ShoppingCartService.Data;
using ShoppingCartService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartService.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateItemAsync(int id, Item item)
        {
            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null)
            {
                return null;
            }

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.StockQuantity = item.StockQuantity;

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}