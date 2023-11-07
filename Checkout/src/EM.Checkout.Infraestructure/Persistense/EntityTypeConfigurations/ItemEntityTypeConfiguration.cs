using EM.Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Checkout.Infraestructure.Persistense.EntityTypeConfigurations;

public sealed class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(x => x.ProductImage)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Ignore(x => x.Amount);
    }
}
