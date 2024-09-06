using System.Security.Claims;

namespace Anomalias.Application.Extensions;

public static class ClaimsPrincipalExtensions
{

    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        if (principal.Identity is not null)
            return principal.Identity.IsAuthenticated;

        return false;
    }



    public static string GetName(this ClaimsPrincipal principal)
    {

        string? name = principal.FindFirstValue(ClaimTypes.Name);

        if (!string.IsNullOrEmpty(name)) return name;

        throw new ApplicationException("User id is unavailable");

    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {

        string? userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var id)) return id;

        throw new ApplicationException("User id is unavailable");

    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrEmpty(email)) return email;

        throw new ApplicationException("User email is unavailable");
    }


    public static string? FindFirstValue(this ClaimsPrincipal principal, string claim)
    {
        return principal.FindFirst(x => x.Type == claim)?.Value;
    }
}