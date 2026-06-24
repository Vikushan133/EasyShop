using EasyShop.Data;
using EasyShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Controllers;

public class ProductsController
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAll()
    {
        return _context.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products.Find(id);
    }

    public Dictionary<int, string> GetProductNames(List<int> ids)
    {
        return _context.Products
            .Where(p => ids.Contains(p.ProductId))
            .ToDictionary(p => p.ProductId, p => p.Name);
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
