using Anomalias.Infrastructure.Identity.Enum;
using Anomalias.Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace Anomalias.Infrastructure.Identity.Extensions;
public class CustomUserClaimsPrincipalFactory(
   UserManager<IdentityUser> userManager,
   RoleManager<Role> roleManager,
   IOptions<IdentityOptions> optionsAccessor) : UserClaimsPrincipalFactory<IdentityUser, Role>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var userRoleNames = await UserManager.GetRolesAsync(user) ?? [];

        var userRoles = await RoleManager.Roles.Where(r =>
            userRoleNames.Contains(r.Name!)).ToListAsync();

        var userPermissions = EPermissions.None;


        foreach (var role in userRoles)
        {
            userPermissions |= role.Permissions;
        }
        var permissionsValue = (int)userPermissions;

        identity.AddClaim(
            new Claim(CustomClaimTypes.Permissions, permissionsValue.ToString()));

        return identity;
    }
}
