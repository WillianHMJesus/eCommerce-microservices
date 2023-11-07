using EM.Payments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Payments.Infraestructure.Persistense;

public sealed class PaymentContext : DbContext
{
    public PaymentContext(DbContextOptions<PaymentContext> options)
        : base(options) { }

    public DbSet<Transaction> Transactions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
