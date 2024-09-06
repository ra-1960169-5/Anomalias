using Anomalia.Application.Abstractions.Services;
using Anomalia.Identity.Configurations;
using Anomalia.Identity.Data;
using Anomalia.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Identity;
public static class DependencyInjection
{
    public static IServiceCollection AddIdentityDepencencyInjection(
   this IServiceCollection services,
   IConfiguration configuration)
    {
        services.AddIdentityDataBaseConfiguration(configuration);
        services.AddIdentityConfiguration();
        services.AddScoped<IIdentityService, IdentityService>();
        services.CreateDatabaseIdentity();
    

        return services;
    }

   internal static IServiceCollection CreateDatabaseIdentity(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var dataContext = provider.GetService<ApplicationIdentityDbContext>();
        dataContext?.Database.EnsureCreated();
        return services;

    }
}
