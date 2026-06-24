using EasyShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasOne(sc => sc.User)
                  .WithMany(u => u.ShoppingCarts)
                  .HasForeignKey(sc => sc.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.Product)
                  .WithMany(p => p.ShoppingCarts)
                  .HasForeignKey(sc => sc.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
