using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRS.BankAPI.Persistence.Configuration;
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("Users_Roles");
        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.Property(user => user.UserId)
        .HasConversion(userId => userId!.Value, value => new UserId(value));

    }
}