using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Data.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
