using System.ComponentModel.DataAnnotations;

namespace EasyShop.Data.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
