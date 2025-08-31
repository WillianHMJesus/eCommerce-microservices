using EM.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Authentication.Infraestructure.EntityTypeConfigurations;

public sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Role.NameMaxLenght);

        builder.HasMany(x => x.Profiles)
            .WithMany(x => x.Roles)
            .UsingEntity("Profile_Roles",
                x => x.HasOne(typeof(Profile)).WithMany().HasForeignKey("ProfileId").HasConstraintName("FK_Profile_Roles_Profiles"),
                x => x.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasConstraintName("FK_Profile_Roles_Roles"));
    }
}
