using Anomalias.Application.Abstractions.Services;
using Anomalias.Infrastructure.Identity.Data;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Identity.Configurations;
public static class IdentityDataInitializer
{
    private const string _ADMINISTRADOR_ROLE = "ADMINISTRADOR";
    private const string _USER_ROLE = "USUARIO";
    private static IIdentityService _identityService = null!;
    private readonly static IList<(string name, int value)> permissions = [new(_ADMINISTRADOR_ROLE, (int)EPermissions.GERENCIADOR), new(_USER_ROLE, (int)EPermissions.ANOMALIA)];
    public async static Task SeedDataIdentity(this IServiceCollection services)
    {
        try
        {
            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                context.Database.EnsureCreated();
            _identityService = serviceProvider.GetRequiredService<IIdentityService>();
            await SeedRoles();
            await SeedUsers();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    internal async static Task SeedUsers()
    {
        const string EMAIL = "ADMIN@ANOMALIA.COM.BR";
        const string PASSWORD = "Admin@123";

        if (await _identityService.IsEmailUniqueAsync(EMAIL))
        {
            var userIdentity = await _identityService.RegisterUserAsync(EMAIL, PASSWORD);
            if (userIdentity.IsSuccess) await _identityService.AddRoleToUserAsync(userIdentity.Value, _ADMINISTRADOR_ROLE);
        }
    }
    internal async static Task SeedRoles()
    {
        foreach (var (name, value) in permissions)
        {
            if (!await _identityService.IsRoleExistAsync(name)) await _identityService.CreateRoleAsync(name, value);
        }
    }
}
