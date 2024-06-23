using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
