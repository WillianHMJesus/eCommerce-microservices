using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Authentication.Infraestructure.EntityTypeConfigurations;

public sealed class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(Profile.NameMaxLenght);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Profiles)
            .UsingEntity("User_Profiles",
                x => x.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasConstraintName("FK_User_Profiles_Users"),
                x => x.HasOne(typeof(Profile)).WithMany().HasForeignKey("ProfileId").HasConstraintName("FK_User_Profiles_Profiles"));

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Profiles)
            .UsingEntity("Profile_Roles",
                x => x.HasOne(typeof(Profile)).WithMany().HasForeignKey("ProfileId").HasConstraintName("FK_Profile_Roles_Profiles"),
                x => x.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasConstraintName("FK_Profile_Roles_Roles"));
    }
}
