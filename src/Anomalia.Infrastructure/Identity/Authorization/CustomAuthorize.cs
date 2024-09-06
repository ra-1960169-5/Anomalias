using Anomalias.Infrastructure.Identity.Enum;
using Anomalias.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Anomalias.Infrastructure.Identity.Authorization;

public class CustomAuthorization
{
    public static bool ValidarPermission(HttpContext context, EPermissions permissions)
    {

        var permissionClaim = context.User.FindFirst(
            c => c.Type == CustomClaimTypes.Permissions);
        if (permissionClaim == null) return false;
        if (!int.TryParse(permissionClaim.Value, out int permissionClaimValue)) return false;
        var userPermissions = (EPermissions)permissionClaimValue;
        var requirement = permissions;
        if ((userPermissions & requirement) != 0) return true;
        return false;
    }

}

public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute(EPermissions permissions) : base(typeof(RequisitoCustomFilter))
    {
        Arguments = [permissions];
    }
}

public class RequisitoCustomFilter(EPermissions permissions) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Account", Action = "Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
            return;
        }

        if (!CustomAuthorization.ValidarPermission(context.HttpContext, permissions))
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}

