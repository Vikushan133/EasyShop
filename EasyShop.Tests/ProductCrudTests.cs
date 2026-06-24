using EasyShop.Controllers;
using EasyShop.Data;
using EasyShop.Data.Models;
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
        var controller = new ProductsController(context);

        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.99m,
            Quantity = 5,
            Category = "Test"
        };

        controller.Add(product);

        Assert.That(product.ProductId, Is.GreaterThan(0));
    }

    [Test]
    public void GetProductById_ShouldReturnCorrectProduct()
    {
        var context = CreateContext("Test_GetProduct");
        var controller = new ProductsController(context);

        var product = new Product
        {
            Name = "Laptop",
            Description = "High performance laptop",
            Price = 2499.99m,
            Quantity = 10,
            Category = "Electronics"
        };

        controller.Add(product);
        var result = controller.GetById(product.ProductId);

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
        var controller = new ProductsController(context);

        var product = new Product
        {
            Name = "Old Name",
            Description = "Old Description",
            Price = 100m,
            Quantity = 1,
            Category = "Old"
        };

        controller.Add(product);

        product.Name = "Updated Name";
        product.Price = 200m;
        product.Quantity = 5;
        controller.Update(product);

        var updated = controller.GetById(product.ProductId);

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
        var controller = new ProductsController(context);

        var product = new Product
        {
            Name = "To Delete",
            Description = "Will be deleted",
            Price = 50m,
            Quantity = 2,
            Category = "Test"
        };

        controller.Add(product);
        Assert.That(controller.GetAll().Count, Is.EqualTo(1));

        controller.Delete(product.ProductId);

        var allProducts = controller.GetAll();
        Assert.That(allProducts.Count, Is.EqualTo(0));
    }
}
