using EM.Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Checkout.Infraestructure.Persistense;

public sealed class CheckoutContext : DbContext
{
    public CheckoutContext(DbContextOptions<CheckoutContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CheckoutContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
