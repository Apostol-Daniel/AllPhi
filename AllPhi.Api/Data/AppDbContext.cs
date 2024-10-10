using AllPhi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Customer>(customerEntity =>
        {
            customerEntity.HasKey(e => e.Id);
            customerEntity.HasIndex(e => e.Email).IsUnique();
            customerEntity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            customerEntity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            customerEntity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        });

        builder.Entity<Order>(orderEntity =>
        {
            orderEntity.HasKey(e => e.Id);
            orderEntity.Property(e => e.Description).IsRequired().HasMaxLength(100).IsUnicode();
            orderEntity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            orderEntity.Property(e => e.CreationDate).IsRequired();
            orderEntity.HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

