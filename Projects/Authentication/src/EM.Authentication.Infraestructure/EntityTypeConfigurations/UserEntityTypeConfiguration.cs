using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EM.Authentication.Infraestructure.EntityTypeConfigurations;

public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(User.UserNameMaxLenght);

        builder.OwnsOne(x => x.Email, emailBuilder =>
        {
            emailBuilder
                .Property(x => x.EmailAddress)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(Email.EmailAddressMaxLenght)
                .HasColumnName("EmailAddress");
        });

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(200);

        builder.HasMany(x => x.Profiles)
            .WithMany(x => x.Users)
            .UsingEntity("User_Profiles",
                x => x.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasConstraintName("FK_User_Profiles_Users"),
                x => x.HasOne(typeof(Profile)).WithMany().HasForeignKey("ProfileId").HasConstraintName("FK_User_Profiles_Profiles"));
    }
}
