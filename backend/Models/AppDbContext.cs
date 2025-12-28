using GestaoPedidosLynx.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidosLynx.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("customers");
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<OrderItem>().ToTable("order_items");
        modelBuilder.Entity<Payment>().ToTable("payments");

        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(160);
        modelBuilder.Entity<Customer>().Property(c => c.Name).HasMaxLength(120);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(120);
        modelBuilder.Entity<Product>().Property(p => p.Category).HasMaxLength(60);
        modelBuilder.Entity<Product>().Property(p => p.Active).HasDefaultValue(true);
        modelBuilder.Entity<Order>().Property(o => o.Status).HasMaxLength(20);
        modelBuilder.Entity<Payment>().Property(p => p.Method).HasMaxLength(20);
    }
}