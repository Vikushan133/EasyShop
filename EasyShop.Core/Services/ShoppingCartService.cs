using EasyShop.Core.Contracts;
using EasyShop.Core.Models;

namespace EasyShop.Core.Services;

public class ShoppingCartService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddToCart(int userId, int productId, int count)
    {
        var product = _unitOfWork.Products.GetById(productId);
        if (product == null)
            throw new ArgumentException("Product not found.");

        var cartItem = new ShoppingCart
        {
            UserId = userId,
            ProductId = productId,
            ProductCount = count,
            DateAdded = DateTime.Now,
            TotalPrice = product.Price * count
        };

        _unitOfWork.ShoppingCarts.Add(cartItem);
        _unitOfWork.Complete();
    }

    public IEnumerable<ShoppingCart> GetCartByUser(int userId)
    {
        return _unitOfWork.ShoppingCarts.Find(sc => sc.UserId == userId);
    }

    public decimal CalculateTotal(int userId)
    {
        var items = GetCartByUser(userId);
        return items.Sum(i => i.TotalPrice);
    }
}
