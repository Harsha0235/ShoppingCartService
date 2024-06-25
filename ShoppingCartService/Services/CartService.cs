using ShoppingCartService.Models;
using ShoppingCartService.Data;
using ShoppingCartService.Exceptions;
using ShoppingCartService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartService.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<CartItem> AddItemToCartAsync(string userId, int itemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
            {
                throw new ArgumentException("We couldn't find the item you're trying to add. It might have been removed from our inventory.");
            }

            if (item.StockQuantity < quantity)
            {
                throw new OutOfStockException($"Sorry, we only have {item.StockQuantity} of this item in stock.");
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (cartItem != null)
            {
                if (item.StockQuantity < cartItem.Quantity + quantity)
                {
                    throw new OutOfStockException($"Sorry, we don't have enough stock. You can add up to {item.StockQuantity - cartItem.Quantity} more of this item.");
                }
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem { ItemId = itemId, Item = item, Quantity = quantity };
                cart.Items.Add(cartItem);
            }

            item.StockQuantity -= quantity;
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(string userId, int itemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (cartItem == null)
            {
                throw new ArgumentException("This item isn't in your cart. Try adding it first.");
            }

            var item = await _context.Items.FindAsync(itemId);
            var quantityDifference = quantity - cartItem.Quantity;

            if (quantityDifference > 0 && item.StockQuantity < quantityDifference)
            {
                throw new OutOfStockException($"Sorry, we don't have enough stock. You can add up to {item.StockQuantity} more of this item.");
            }

            cartItem.Quantity = quantity;
            item.StockQuantity -= quantityDifference;

            if (cartItem.Quantity <= 0)
            {
                cart.Items.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task RemoveItemFromCartAsync(string userId, int itemId)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (cartItem == null)
            {
                throw new ArgumentException("This item isn't in your cart.");
            }

            var item = await _context.Items.FindAsync(itemId);
            item.StockQuantity += cartItem.Quantity;
            cart.Items.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}