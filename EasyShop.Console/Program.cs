using EasyShop.Core.Models;
using EasyShop.Core.Services;
using EasyShop.Data;
using EasyShop.Data.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connection)
    .Options;

using var context = new AppDbContext(options);
context.Database.EnsureCreated();

var unitOfWork = new UnitOfWork(context);
var productService = new ProductService(unitOfWork);
var userService = new UserService(unitOfWork);
var cartService = new ShoppingCartService(unitOfWork);

while (true)
{
    if (!Console.IsInputRedirected)
        Console.Clear();
    Console.WriteLine("=== EasyShop ===");
    Console.WriteLine("1. Добавяне на продукт");
    Console.WriteLine("2. Преглед на всички продукти");
    Console.WriteLine("3. Редактиране на продукт");
    Console.WriteLine("4. Изтриване на продукт");
    Console.WriteLine("5. Създаване на потребител");
    Console.WriteLine("6. Добавяне на продукт в количка");
    Console.WriteLine("7. Преглед на количката");
    Console.WriteLine("0. Изход");
    Console.Write("Изберете опция: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddProduct();
            break;
        case "2":
            ViewAllProducts();
            break;
        case "3":
            EditProduct();
            break;
        case "4":
            DeleteProduct();
            break;
        case "5":
            CreateUser();
            break;
        case "6":
            AddToCart();
            break;
        case "7":
            ViewCart();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Невалидна опция!");
            break;
    }

    if (!Console.IsInputRedirected)
    {
        Console.WriteLine("Натиснете произволен клавиш...");
        Console.ReadKey();
    }
}

void AddProduct()
{
    Console.Write("Име: ");
    var name = Console.ReadLine() ?? "";
    Console.Write("Описание: ");
    var description = Console.ReadLine() ?? "";
    Console.Write("Цена: ");
    decimal price = decimal.Parse(Console.ReadLine() ?? "0");
    Console.Write("Количество: ");
    int quantity = int.Parse(Console.ReadLine() ?? "0");
    Console.Write("Категория: ");
    var category = Console.ReadLine() ?? "";

    productService.AddProduct(new Product
    {
        Name = name,
        Description = description,
        Price = price,
        Quantity = quantity,
        Category = category
    });

    Console.WriteLine("Продуктът е добавен успешно!");
}

void ViewAllProducts()
{
    var products = productService.GetAllProducts();
    if (!products.Any())
    {
        Console.WriteLine("Няма налични продукти.");
        return;
    }

    foreach (var p in products)
    {
        Console.WriteLine($"[{p.ProductId}] {p.Name} - {p.Price:F2} лв. (Количество: {p.Quantity}, Категория: {p.Category})");
        Console.WriteLine($"   {p.Description}");
    }
}

void EditProduct()
{
    Console.Write("ID на продукта за редактиране: ");
    int id = int.Parse(Console.ReadLine() ?? "0");

    var product = productService.GetProductById(id);
    if (product == null)
    {
        Console.WriteLine("Продуктът не е намерен.");
        return;
    }

    Console.Write($"Ново име ({product.Name}): ");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

    Console.Write($"Ново описание ({product.Description}): ");
    var description = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(description)) product.Description = description;

    Console.Write($"Нова цена ({product.Price}): ");
    var priceInput = Console.ReadLine();
    if (decimal.TryParse(priceInput, out var price)) product.Price = price;

    Console.Write($"Ново количество ({product.Quantity}): ");
    var qtyInput = Console.ReadLine();
    if (int.TryParse(qtyInput, out var qty)) product.Quantity = qty;

    Console.Write($"Нова категория ({product.Category}): ");
    var category = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(category)) product.Category = category;

    productService.UpdateProduct(product);
    Console.WriteLine("Продуктът е обновен успешно!");
}

void DeleteProduct()
{
    Console.Write("ID на продукта за изтриване: ");
    int id = int.Parse(Console.ReadLine() ?? "0");

    productService.DeleteProduct(id);
    Console.WriteLine("Продуктът е изтрит успешно!");
}

void CreateUser()
{
    Console.Write("Име: ");
    var firstName = Console.ReadLine() ?? "";
    Console.Write("Фамилия: ");
    var lastName = Console.ReadLine() ?? "";
    Console.Write("Email: ");
    var email = Console.ReadLine() ?? "";
    Console.Write("Парола: ");
    var password = Console.ReadLine() ?? "";
    Console.Write("Адрес: ");
    var address = Console.ReadLine() ?? "";

    userService.CreateUser(new User
    {
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        Password = password,
        Address = address
    });

    Console.WriteLine("Потребителят е създаден успешно!");
}

void AddToCart()
{
    Console.Write("ID на потребител: ");
    int userId = int.Parse(Console.ReadLine() ?? "0");
    Console.Write("ID на продукт: ");
    int productId = int.Parse(Console.ReadLine() ?? "0");
    Console.Write("Брой: ");
    int count = int.Parse(Console.ReadLine() ?? "0");

    try
    {
        cartService.AddToCart(userId, productId, count);
        Console.WriteLine("Продуктът е добавен в количката!");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void ViewCart()
{
    Console.Write("ID на потребител: ");
    int userId = int.Parse(Console.ReadLine() ?? "0");

    var items = cartService.GetCartByUser(userId);
    if (!items.Any())
    {
        Console.WriteLine("Количката е празна.");
        return;
    }

    foreach (var item in items)
    {
        Console.WriteLine($"[{item.CartId}] Продукт ID: {item.ProductId}, Брой: {item.ProductCount}, " +
                          $"Общо: {item.TotalPrice:F2} лв., Добавен на: {item.DateAdded:dd.MM.yyyy HH:mm}");
    }

    Console.WriteLine($"Обща стойност: {cartService.CalculateTotal(userId):F2} лв.");
}
