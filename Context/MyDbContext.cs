using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Context;
using Models;

public class MyDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().HasKey(c => c.IdClient);
        modelBuilder.Entity<Subscription>().HasKey(s => s.IdSubscription);
        modelBuilder.Entity<Sale>().HasKey(s => s.IdSale);
        modelBuilder.Entity<Discount>().HasKey(d => d.IdDiscount);
        modelBuilder.Entity<Payment>().HasKey(p => p.IdPayment);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Sales)
            .WithOne(s => s.Client)
            .HasForeignKey(s => s.IdClient);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Payments)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.IdClient);

        modelBuilder.Entity<Subscription>()
            .HasMany(s => s.Sales)
            .WithOne(s => s.Subscription)
            .HasForeignKey(s => s.IdSubscription);

        modelBuilder.Entity<Subscription>()
            .HasMany(s => s.Discounts)
            .WithOne(d => d.Subscription)
            .HasForeignKey(d => d.IdSubscription);

        modelBuilder.Entity<Subscription>()
            .HasMany(s => s.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.IdSubscription);
    }
}
