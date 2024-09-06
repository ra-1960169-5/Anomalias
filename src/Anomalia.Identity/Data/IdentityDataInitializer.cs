using Anomalia.Identity.Enum;
using Anomalia.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Identity.Data;
public static class IdentityDataInitializer
{
    private const string _ADMINISTRADOR_ROLE = "ADMINISTRADOR";
    private const string _USER_ROLE = "USUARIO";
    public async static Task SeedDataIdentity(this IServiceCollection services)
    {
        try
        {
            var provider = services.BuildServiceProvider();
            var roleManager = provider.GetRequiredService<RoleManager<Role>>();
            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    public async static Task SeedUsers(UserManager<IdentityUser> userManager)
    {
        const string NAME = "ADMINISTRADOR";
        const string EMAIL = "ADMIN@ANOMALIA.COM.BR";
        const string PASSWORD = "Admin@123";

        if (await userManager.FindByNameAsync(NAME) is null || await userManager.FindByEmailAsync(EMAIL) is null)
        {
            IdentityUser user = new()
            {
                UserName = NAME,
                Email = EMAIL,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(user, PASSWORD);
            if (result.Succeeded) await userManager.AddToRoleAsync(user, _ADMINISTRADOR_ROLE);
        }
    }
    public async static Task SeedRoles(RoleManager<Role> roleManager)
    {

        if (!await roleManager.RoleExistsAsync(_ADMINISTRADOR_ROLE)) await roleManager.CreateAsync(new Role(_ADMINISTRADOR_ROLE, EPermissions.GERENCIADOR));

        if (!await roleManager.RoleExistsAsync(_USER_ROLE)) await roleManager.CreateAsync(new Role(_USER_ROLE, EPermissions.ANOMALIA));
    }
}
