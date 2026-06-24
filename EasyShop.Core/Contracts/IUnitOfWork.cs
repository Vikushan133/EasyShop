using EasyShop.Core.Models;

namespace EasyShop.Core.Contracts;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<User> Users { get; }
    IRepository<ShoppingCart> ShoppingCarts { get; }
    void Complete();
}
