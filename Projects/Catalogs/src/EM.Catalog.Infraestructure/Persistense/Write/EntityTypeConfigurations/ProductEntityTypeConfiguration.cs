using EM.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Catalog.Infraestructure.Persistense.Write.EntityTypeConfigurations;

public sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Product.NameMaxLenght);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Product.DescriptionMaxLenght);

        builder.Property(x => x.Image)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Product.ImageMaxLenght);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();
    }
}
