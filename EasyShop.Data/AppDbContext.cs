using EasyShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=EasyShop.db");
    }

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
