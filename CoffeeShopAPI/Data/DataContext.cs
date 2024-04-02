using CoffeeShopAPI.Data.dto.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Data;

public class DataContext : DbContext
{
    public DbSet<User?> Users { get; set; }
    public DbSet<MenuItem?> MenuItems { get; set; }
    public DbSet<Order?> Orders { get; set; }
    public DbSet<OrderItem?> OrderItems { get; set; }
    public DbSet<Additive?> Additives { get; set; }
    public DbSet<MenuItemAdditive?> MenuItemAdditives { get; set; }
    public DbSet<OrderItemAdditive?> OrderItemAdditives { get; set; }
    public DbSet<Category?> Categories { get; set; }
    
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItemAdditive>()
            .HasKey(t => new { t.MenuItemId, t.AdditiveId });
        modelBuilder.Entity<OrderItemAdditive>()
            .HasKey(t => new { t.OrderItemId, t.AdditiveId });

        modelBuilder.Entity<Order>()
            .HasMany<OrderItem>(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.Id);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne<MenuItem>(oi => oi.Item)
            .WithMany()
            .HasForeignKey(oi => oi.ItemId);
        
        modelBuilder.Entity<OrderItem>()
            .HasMany<OrderItemAdditive>(oi => oi.Additives)
            .WithOne(oia => oia.OrderItem)
            .HasForeignKey(oia => oia.OrderItemId);
        modelBuilder.Entity<OrderItemAdditive>()
            .HasOne(pt => pt.Additive)
            .WithMany()
            .HasForeignKey(pt => pt.AdditiveId);
        

        modelBuilder.Entity<MenuItem>()
            .HasOne<Category>(mi => mi.Category)
            .WithMany()
            .HasForeignKey(mi => mi.CategoryId);
        
        modelBuilder.Entity<MenuItem>()
            .HasMany<MenuItemAdditive>(mi => mi.AvailableAdditives)
            .WithOne(mia => mia.MenuItem)
            .HasForeignKey(mia => mia.MenuItemId);
        modelBuilder.Entity<MenuItemAdditive>()
            .HasOne(pt => pt.Additive)
            .WithMany()
            .HasForeignKey(pt => pt.AdditiveId);


    }
}