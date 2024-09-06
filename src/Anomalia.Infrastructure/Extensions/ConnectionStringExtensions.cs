using Microsoft.Extensions.Configuration;

namespace Anomalias.Infrastructure.Extensions;
public static class ConnectionStringExtensions
{
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
    {

        return configuration.GetConnectionString(name) ?? throw new InvalidOperationException($"A string de conexão {name} é inválida");

    }
}
