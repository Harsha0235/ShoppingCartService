using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the item.")]
        [StringLength(100, ErrorMessage = "The item name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide a price for the item.")]
        [Range(0.01, 10000, ErrorMessage = "The price must be between $0.01 and $10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please provide the quantity in stock.")]
        [Range(0, 1000, ErrorMessage = "The stock quantity must be between 0 and 1,000.")]
        public int StockQuantity { get; set; }
    }
}