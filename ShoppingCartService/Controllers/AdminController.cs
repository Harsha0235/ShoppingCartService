using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Models;
using ShoppingCartService.Services.Interfaces;

namespace ShoppingCartService.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IItemService _itemService;

        public AdminController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("We couldn't find the item you're looking for. It might have been removed from our inventory.");
            }
            return Ok(item);
        }

        [HttpPost("items")]
        public async Task<ActionResult<Item>> AddItem(Item item)
        {
            var addedItem = await _itemService.AddItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = addedItem.Id }, addedItem);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            var updatedItem = await _itemService.UpdateItemAsync(id, item);
            if (updatedItem == null)
            {
                return NotFound("We couldn't find the item you're trying to update. It might have been removed from our inventory.");
            }
            return NoContent();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}