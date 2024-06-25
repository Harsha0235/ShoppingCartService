using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Models;
using ShoppingCartService.Services.Interfaces;
using ShoppingCartService.Exceptions;

namespace ShoppingCartService.Controllers
{
    [ApiController]
    [Route("api/user")]
    [ApiExplorerSettings(GroupName = "User")]
    public class UserController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IItemService _itemService;

        public UserController(ICartService cartService, IItemService itemService)
        {
            _cartService = cartService;
            _itemService = itemService;
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("cart/{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Oops! Something went wrong on our end: {ex.Message}");
            }
        }

        [HttpPost("cart/{userId}/items")]
        public async Task<ActionResult<CartItem>> AddItemToCart(string userId, [FromBody] AddToCartRequest request)
        {
            try
            {
                var cartItem = await _cartService.AddItemToCartAsync(userId, request.ItemId, request.Quantity);
                return Ok(cartItem);
            }
            catch (OutOfStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Oops! Something went wrong on our end: {ex.Message}");
            }
        }

        [HttpPut("cart/{userId}/items/{itemId}")]
        public async Task<ActionResult<CartItem>> UpdateCartItem(string userId, int itemId, [FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var cartItem = await _cartService.UpdateCartItemAsync(userId, itemId, request.Quantity);
                return Ok(cartItem);
            }
            catch (OutOfStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Oops! Something went wrong on our end: {ex.Message}");
            }
        }

        [HttpDelete("cart/{userId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(string userId, int itemId)
        {
            try
            {
                await _cartService.RemoveItemFromCartAsync(userId, itemId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Oops! Something went wrong on our end: {ex.Message}");
            }
        }
    }

    public class AddToCartRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}