using Anomalia.Identity.Enum;
using Microsoft.AspNetCore.Identity;

namespace Anomalia.Identity.Model;
public class Role(string Name, EPermissions permissions = EPermissions.None) : IdentityRole(Name)
{
    public EPermissions Permissions { get; set; } = permissions;
}
