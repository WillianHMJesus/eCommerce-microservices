using EM.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
