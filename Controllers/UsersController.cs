using EasyShop.Data;
using EasyShop.Data.Models;

namespace EasyShop.Controllers;

public class UsersController
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public string? GetUserName(int userId)
    {
        var user = _context.Users.Find(userId);
        return user != null ? $"{user.FirstName} {user.LastName}" : null;
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
