using EM.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public class WriteContext : DbContext
{
    public WriteContext(DbContextOptions<WriteContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
