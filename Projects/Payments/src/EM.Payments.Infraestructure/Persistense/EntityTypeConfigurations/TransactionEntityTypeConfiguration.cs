using EM.Payments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Payments.Infraestructure.Persistense.EntityTypeConfigurations;

public sealed class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CardNumber)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(15);
    }
}
