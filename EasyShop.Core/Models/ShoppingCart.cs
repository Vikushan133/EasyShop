using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Core.Models;

public class ShoppingCart
{
    [Key]
    public int CartId { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public int ProductCount { get; set; }

    public DateTime DateAdded { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
