using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Models;
using ShoppingCartService.Services;

namespace ShoppingCartService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetItems()
        {
            var items = await _cartService.GetCartItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetItem(int id)
        {
            var item = await _cartService.GetCartItemByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> AddItem(CartItem item)
        {
            await _cartService.AddItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, CartItem item)
        {
            await _cartService.UpdateItemAsync(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _cartService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}