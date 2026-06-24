using EasyShop.Core.Contracts;
using EasyShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Data.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IRepository<Product>? _products;
    private IRepository<User>? _users;
    private IRepository<ShoppingCart>? _shoppingCarts;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products =>
        _products ??= new Repository<Product>(_context);

    public IRepository<User> Users =>
        _users ??= new Repository<User>(_context);

    public IRepository<ShoppingCart> ShoppingCarts =>
        _shoppingCarts ??= new Repository<ShoppingCart>(_context);

    public void Complete()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
