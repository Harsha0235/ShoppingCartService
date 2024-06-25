using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required(ErrorMessage = "Please specify the quantity.")]
        [Range(1, 100, ErrorMessage = "You can add between 1 and 100 items to your cart.")]
        public int Quantity { get; set; }
    }
}