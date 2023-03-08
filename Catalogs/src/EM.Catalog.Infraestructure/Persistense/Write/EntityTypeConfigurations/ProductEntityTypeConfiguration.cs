using EM.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Catalog.Infraestructure.Persistense.Write.EntityTypeConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(x => x.Image)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property<Guid>("CategoryId");
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey("CategoryId")
            .IsRequired();
    }
}
