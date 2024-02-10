using CQRS.BankAPI.Domain.Permissions;

namespace CQRS.BankAPI.Domain.Roles;

public sealed class RolePermission
{

    public int RoleId { get; set; }
    public PermissionId? PermissionId { get; set; }

}