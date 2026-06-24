using EasyShop.Data.Models;

namespace EasyShop.Views;

public static class ConsoleView
{
    public static string ShowMainMenu()
    {
        if (!Console.IsInputRedirected)
            Console.Clear();
        Console.WriteLine("=== EasyShop ===");
        Console.WriteLine("1. Add Product");
        Console.WriteLine("2. View All Products");
        Console.WriteLine("3. Edit Product");
        Console.WriteLine("4. Delete Product");
        Console.WriteLine("5. Create User");
        Console.WriteLine("6. Add Product to Cart");
        Console.WriteLine("7. View Cart");
        Console.WriteLine("8. Remove Items from Cart");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");
        return Console.ReadLine() ?? "";
    }

    public static Product GetProductInput()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Description: ");
        var description = Console.ReadLine() ?? "";
        Console.Write("Price: ");
        var price = decimal.Parse(Console.ReadLine() ?? "0");
        Console.Write("Quantity: ");
        var quantity = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Category: ");
        var category = Console.ReadLine() ?? "";

        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            Quantity = quantity,
            Category = category
        };
    }

    public static void ShowAllProducts(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("No products available.");
            return;
        }

        foreach (var p in products)
        {
            Console.WriteLine($"[{p.ProductId}] {p.Name} - {p.Price:F2} BGN (Qty: {p.Quantity}, Category: {p.Category})");
            Console.WriteLine($"   {p.Description}");
        }
    }

    public static int GetProductId()
    {
        Console.Write("Product ID: ");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    public static void EditProduct(Product product)
    {
        Console.Write($"New name ({product.Name}): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

        Console.Write($"New description ({product.Description}): ");
        var desc = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(desc)) product.Description = desc;

        Console.Write($"New price ({product.Price}): ");
        var pInput = Console.ReadLine();
        if (decimal.TryParse(pInput, out var price)) product.Price = price;

        Console.Write($"New quantity ({product.Quantity}): ");
        var qInput = Console.ReadLine();
        if (int.TryParse(qInput, out var qty)) product.Quantity = qty;

        Console.Write($"New category ({product.Category}): ");
        var cat = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(cat)) product.Category = cat;
    }

    public static int GetDeleteId()
    {
        Console.Write("Product ID to delete: ");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    public static User GetUserInput()
    {
        Console.Write("First name: ");
        var fn = Console.ReadLine() ?? "";
        Console.Write("Last name: ");
        var ln = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        var pwd = Console.ReadLine() ?? "";
        Console.Write("Address: ");
        var addr = Console.ReadLine() ?? "";

        return new User
        {
            FirstName = fn,
            LastName = ln,
            Email = email,
            Password = pwd,
            Address = addr
        };
    }

    public static (int userId, int productId, int count) GetAddToCartInput()
    {
        Console.Write("User ID: ");
        var userId = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Product ID: ");
        var productId = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Count: ");
        var count = int.Parse(Console.ReadLine() ?? "0");

        return (userId, productId, count);
    }

    public static int GetRemoveQuantity()
    {
        Console.Write("Quantity to remove: ");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    public static bool GetConfirmation(string prompt)
    {
        Console.Write($"{prompt} (Y/N): ");
        var input = Console.ReadLine()?.Trim().ToUpper();
        return input == "Y";
    }

    public static int GetUserId()
    {
        Console.Write("User ID: ");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    public static void ShowCart(List<ShoppingCart> items, decimal total, string userName, Dictionary<int, string> productNames)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.WriteLine($"Cart for: {userName}");

        foreach (var item in items)
        {
            var productName = productNames.GetValueOrDefault(item.ProductId, "Unknown");
            Console.WriteLine($"[{item.CartId}] {productName} (ID: {item.ProductId}), Count: {item.ProductCount}, " +
                              $"Total: {item.TotalPrice:F2} BGN, Added: {item.DateAdded:dd.MM.yyyy HH:mm}");
        }

        Console.WriteLine($"Total cart value: {total:F2} BGN.");
    }

    public static void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public static void ShowError(string error)
    {
        Console.WriteLine($"Error: {error}");
    }

    public static void WaitForKey()
    {
        Console.WriteLine("Press any key...");
        try
        {
            Console.ReadKey();
        }
        catch (InvalidOperationException)
        {
            Console.ReadLine();
        }
    }
}
