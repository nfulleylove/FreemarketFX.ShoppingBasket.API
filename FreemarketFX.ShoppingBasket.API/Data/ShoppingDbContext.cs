using FreemarketFX.ShoppingBasket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FreemarketFX.ShoppingBasket.API.Data;

public class ShoppingDbContext : DbContext
{
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseInMemoryDatabase("ShoppingDatabase");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Basket>();

        modelBuilder.Entity<Product>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<BasketProduct>()
            .HasKey(x => new { x.BasketId, x.ProductId });

        modelBuilder.Entity<BasketProduct>()
            .HasOne(x => x.Basket)
            .WithMany(b => b.BasketProducts)
            .HasForeignKey(x => x.BasketId);

        modelBuilder.Entity<BasketProduct>()
            .HasOne(x => x.Product)
            .WithMany(p => p.BasketProducts)
            .HasForeignKey(x => x.ProductId);
    }
}
