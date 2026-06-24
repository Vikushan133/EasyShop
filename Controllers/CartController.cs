using EasyShop.Data;
using EasyShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Controllers;

public class CartController
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    public void AddToCart(int userId, int productId, int count)
    {
        var product = _context.Products.Find(productId);
        if (product == null)
            throw new ArgumentException("Product not found.");

        if (product.Quantity < count)
            throw new ArgumentException($"Not enough stock. Available: {product.Quantity}, requested: {count}.");

        var cartItem = new ShoppingCart
        {
            UserId = userId,
            ProductId = productId,
            ProductCount = count,
            DateAdded = DateTime.Now,
            TotalPrice = product.Price * count
        };

        _context.ShoppingCarts.Add(cartItem);
        _context.SaveChanges();
    }

    public List<ShoppingCart> GetCartByUser(int userId)
    {
        return _context.ShoppingCarts
            .Where(sc => sc.UserId == userId)
            .ToList();
    }

    public void RemoveFromCart(int userId, int productId, int count)
    {
        var item = _context.ShoppingCarts
            .FirstOrDefault(sc => sc.UserId == userId && sc.ProductId == productId);
        if (item == null)
            throw new ArgumentException("Item not found in cart.");
        if (count > item.ProductCount)
            throw new ArgumentException($"Not enough items in cart. In cart: {item.ProductCount}, requested to remove: {count}.");

        var unitPrice = item.TotalPrice / item.ProductCount;
        item.ProductCount -= count;
        item.TotalPrice = unitPrice * item.ProductCount;

        if (item.ProductCount == 0)
            _context.ShoppingCarts.Remove(item);

        _context.SaveChanges();
    }

    public decimal CalculateTotal(int userId)
    {
        return _context.ShoppingCarts
            .Where(sc => sc.UserId == userId)
            .Sum(sc => sc.TotalPrice);
    }
}
