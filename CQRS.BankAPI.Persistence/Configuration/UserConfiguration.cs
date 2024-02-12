using CQRS.BankAPI.Domain;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRS.BankAPI.Persistence.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(
        EntityTypeBuilder<User> builder
        )
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
        .HasConversion(userId => userId!.Value, value => new UserId(value));

        builder.Property(user => user.Name)
        .HasMaxLength(200)
        .HasColumnType("nvarchar")
        .IsRequired();

        builder.Property(user => user.LastName)
        .HasMaxLength(200)
        .HasColumnType("nvarchar")
        .IsRequired();

        builder.Property(user => user.Dni)
        .HasMaxLength(10)
        .HasColumnType("nvarchar")
        .HasConversion(dni => dni!.Value, value => new Dni(value));

        builder.Property(user => user.PhoneNumber)
        .HasMaxLength(20)
        .HasColumnType("nvarchar")
        .HasConversion(phoneNumber => phoneNumber!.Value, value => new PhoneNumber(value));

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => email!.Value, value => new Email(value));

        builder.Property(user => user.PasswordHash)
        .HasMaxLength(1000)
        .HasConversion(password => password!.Value, value => new PasswordHash(value));

        builder.Property(user => user.ConfirmPassword)
        .HasMaxLength(1000)
        .HasColumnType("nvarchar");

        builder.Property(user => user.IpUser)
        .HasMaxLength(100)
        .HasColumnType("nvarchar")
        .HasConversion(ipUser => ipUser!.Value, value => new IpUser(value));

        builder.Property(user => user.UserStatus)
        .HasMaxLength(100)
        .HasColumnType("nvarchar")
        .HasConversion(userStatus => userStatus!.Value, value => new UserStatus(value));

        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.Dni).IsUnique();

        builder.HasMany(x => x.Roles)
            .WithMany()
            .UsingEntity<UserRole>();


    }
}