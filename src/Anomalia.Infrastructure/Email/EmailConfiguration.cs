using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Email;
public static class EmailConfiguration
{
    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    }
}
