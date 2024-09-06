using Anomalias.Infrastructure.Identity.Enum;

namespace Anomalias.Infrastructure.Identity.Model;
public static class RoleStore
{
    public static IReadOnlyList<Role> Roles { get; } = [
     new("ADMINISTRADOR", EPermissions.GERENCIADOR),
     new ("USUARIO", EPermissions.ANOMALIA)
     ];

    public static string? GetIdByName(string name)
    {
        return Roles.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefault();
    }
}
