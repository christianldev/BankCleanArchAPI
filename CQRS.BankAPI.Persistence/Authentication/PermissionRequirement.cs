using Microsoft.AspNetCore.Authorization;

namespace CQRS.BankAPI.Persistence.Authentication;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }



}