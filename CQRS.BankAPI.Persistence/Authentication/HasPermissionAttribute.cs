using CQRS.BankAPI.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace CQRS.BankAPI.Persistence.Authentication;


public class HasPermissionAttribute : AuthorizeAttribute
{

    public HasPermissionAttribute(PermissionEnum permission)
    : base(policy: permission.ToString())
    {

    }

}