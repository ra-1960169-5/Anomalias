using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Identity;

namespace Anomalias.Infrastructure.Identity.Model;
public class Role(string Name, EPermissions permissions = EPermissions.None) : IdentityRole(Name)
{
    public EPermissions Permissions { get; set; } = permissions;
}
