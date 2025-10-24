using EM.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Catalog.Infraestructure.Persistense.Write.EntityTypeConfigurations;

public sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Category.NameMaxLenght);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Category.DescriptionMaxLenght);
    }
}
