using ShoppingCartService.Data.Repositories;
using ShoppingCartService.Exceptions;
using ShoppingCartService.Models;

namespace ShoppingCartService.Services
{
    public class ShoppingService : IShoppingCartService
    {
        private readonly ICartRepository _cartRepository;

        public ShoppingService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _cartRepository.GetAllItemsAsync();
        }

        public async Task<CartItem> GetCartItemByIdAsync(int id)
        {
            var item = await _cartRepository.GetItemByIdAsync(id);
            if (item == null)
            {
                throw new NotFoundException($"Cart item with id {id} not found.");
            }
            return item;
        }

        public async Task AddItemAsync(CartItem item)
        {
            await _cartRepository.AddItemAsync(item);
        }

        public async Task UpdateItemAsync(int id, CartItem item)
        {
            var existingItem = await _cartRepository.GetItemByIdAsync(id);
            if (existingItem == null)
            {
                throw new NotFoundException($"Cart item with id {id} not found.");
            }

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.Quantity = item.Quantity;

            await _cartRepository.UpdateItemAsync(existingItem);
        }

        public async Task DeleteItemAsync(int id)
        {
            var existingItem = await _cartRepository.GetItemByIdAsync(id);
            if (existingItem == null)
            {
                throw new NotFoundException($"Cart item with id {id} not found.");
            }

            await _cartRepository.DeleteItemAsync(id);
        }
    }
}