using EasyShop.Controllers;
using EasyShop.Data;
using EasyShop.Views;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=EasyShop.db")
    .Options;

using var context = new AppDbContext(options);
context.Database.EnsureCreated();

var productsController = new ProductsController(context);
var usersController = new UsersController(context);
var cartController = new CartController(context);

while (true)
{
    var choice = ConsoleView.ShowMainMenu();

    switch (choice)
    {
        case "1":
        {
            var product = ConsoleView.GetProductInput();
            productsController.Add(product);
            ConsoleView.ShowMessage("Product added successfully!");
            break;
        }
        case "2":
        {
            var products = productsController.GetAll();
            ConsoleView.ShowAllProducts(products);
            break;
        }
        case "3":
        {
            var id = ConsoleView.GetProductId();
            var product = productsController.GetById(id);
            if (product == null)
            {
                ConsoleView.ShowError("Product not found.");
            }
            else
            {
                ConsoleView.EditProduct(product);
                productsController.Update(product);
                ConsoleView.ShowMessage("Product updated successfully!");
            }
            break;
        }
        case "4":
        {
            var id = ConsoleView.GetDeleteId();
            productsController.Delete(id);
            ConsoleView.ShowMessage("Product deleted successfully!");
            break;
        }
        case "5":
        {
            var user = ConsoleView.GetUserInput();
            usersController.Create(user);
            ConsoleView.ShowMessage("User created successfully!");
            break;
        }
        case "6":
        {
            var (userId, productId, count) = ConsoleView.GetAddToCartInput();
            try
            {
                cartController.AddToCart(userId, productId, count);
                ConsoleView.ShowMessage("Product added to cart!");
            }
            catch (ArgumentException ex)
            {
                ConsoleView.ShowError(ex.Message);
            }
            break;
        }
        case "7":
        {
            var userId = ConsoleView.GetUserId();
            var items = cartController.GetCartByUser(userId);
            var total = cartController.CalculateTotal(userId);
            var userName = usersController.GetUserName(userId) ?? $"User {userId}";
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            var productNames = productsController.GetProductNames(productIds);
            ConsoleView.ShowCart(items, total, userName, productNames);
            break;
        }
        case "8":
        {
            var userId = ConsoleView.GetUserId();
            var items = cartController.GetCartByUser(userId);
            if (items.Count == 0)
            {
                ConsoleView.ShowMessage("Cart is empty.");
                break;
            }

            var total = cartController.CalculateTotal(userId);
            var userName = usersController.GetUserName(userId) ?? $"User {userId}";
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            var productNames = productsController.GetProductNames(productIds);
            ConsoleView.ShowCart(items, total, userName, productNames);

            var productId = ConsoleView.GetProductId();
            var cartItem = items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                ConsoleView.ShowError("Item not found in cart.");
                break;
            }

            var productName = productNames.GetValueOrDefault(productId, "Unknown");
            var count = ConsoleView.GetRemoveQuantity();

            if (count > cartItem.ProductCount)
            {
                ConsoleView.ShowError($"Not enough items in cart. In cart: {cartItem.ProductCount}, requested to remove: {count}.");
                break;
            }

            if (!ConsoleView.GetConfirmation($"Remove {count}x {productName} from cart"))
            {
                ConsoleView.ShowMessage("Removal cancelled.");
                break;
            }

            try
            {
                cartController.RemoveFromCart(userId, productId, count);
                ConsoleView.ShowMessage("Items removed from cart!");
            }
            catch (ArgumentException ex)
            {
                ConsoleView.ShowError(ex.Message);
            }
            break;
        }
        case "0":
            return;
        default:
            ConsoleView.ShowError("Invalid option!");
            break;
    }

    ConsoleView.WaitForKey();
}
