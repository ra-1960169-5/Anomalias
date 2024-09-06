using Anomalias.App.Configuration;
using Anomalias.Application;
using Anomalias.Infrastructure;

namespace Anomalias.App;

public static class DependencyInjection
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(configuration,env);
        return services;
    }
}
