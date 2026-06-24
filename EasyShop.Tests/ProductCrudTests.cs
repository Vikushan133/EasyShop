using EasyShop.Core.Models;
using EasyShop.Core.Services;
using EasyShop.Data;
using EasyShop.Data.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Tests;

public class ProductCrudTests
{
    private AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new AppDbContext(options);
    }

    [Test]
    public void AddProduct_ShouldSaveToDatabase()
    {
        var context = CreateContext("Test_AddProduct");
        var unitOfWork = new UnitOfWork(context);
        var service = new ProductService(unitOfWork);

        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.99m,
            Quantity = 5,
            Category = "Test"
        };

        service.AddProduct(product);

        Assert.That(product.ProductId, Is.GreaterThan(0));
    }

    [Test]
    public void GetProductById_ShouldReturnCorrectProduct()
    {
        var context = CreateContext("Test_GetProduct");
        var unitOfWork = new UnitOfWork(context);
        var service = new ProductService(unitOfWork);

        var product = new Product
        {
            Name = "Laptop",
            Description = "High performance laptop",
            Price = 2499.99m,
            Quantity = 10,
            Category = "Electronics"
        };

        service.AddProduct(product);
        var result = service.GetProductById(product.ProductId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Name, Is.EqualTo("Laptop"));
            Assert.That(result.Price, Is.EqualTo(2499.99m));
            Assert.That(result.Category, Is.EqualTo("Electronics"));
        });
    }

    [Test]
    public void UpdateProduct_ShouldModifyExistingProduct()
    {
        var context = CreateContext("Test_UpdateProduct");
        var unitOfWork = new UnitOfWork(context);
        var service = new ProductService(unitOfWork);

        var product = new Product
        {
            Name = "Old Name",
            Description = "Old Description",
            Price = 100m,
            Quantity = 1,
            Category = "Old"
        };

        service.AddProduct(product);

        product.Name = "Updated Name";
        product.Price = 200m;
        product.Quantity = 5;
        service.UpdateProduct(product);

        var updated = service.GetProductById(product.ProductId);

        Assert.That(updated, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updated!.Name, Is.EqualTo("Updated Name"));
            Assert.That(updated.Price, Is.EqualTo(200m));
            Assert.That(updated.Quantity, Is.EqualTo(5));
        });
    }

    [Test]
    public void DeleteProduct_ShouldRemoveFromDatabase()
    {
        var context = CreateContext("Test_DeleteProduct");
        var unitOfWork = new UnitOfWork(context);
        var service = new ProductService(unitOfWork);

        var product = new Product
        {
            Name = "To Delete",
            Description = "Will be deleted",
            Price = 50m,
            Quantity = 2,
            Category = "Test"
        };

        service.AddProduct(product);
        Assert.That(service.GetAllProducts().Count(), Is.EqualTo(1));

        service.DeleteProduct(product.ProductId);

        var allProducts = service.GetAllProducts();
        Assert.That(allProducts.Count(), Is.EqualTo(0));
    }
}
