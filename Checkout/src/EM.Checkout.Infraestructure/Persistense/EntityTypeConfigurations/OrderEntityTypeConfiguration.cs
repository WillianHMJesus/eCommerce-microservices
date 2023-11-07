using EM.Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Checkout.Infraestructure.Persistense.EntityTypeConfigurations;

public sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Ignore(x => x.Amount);
    }
}
