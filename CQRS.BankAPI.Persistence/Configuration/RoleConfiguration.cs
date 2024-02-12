using CQRS.BankAPI.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRS.BankAPI.Persistence.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);

        builder.HasData(Role.GetValues());

        builder.HasMany(x => x.Permissions)
        .WithMany()
        .UsingEntity<RolePermission>();

    }
}