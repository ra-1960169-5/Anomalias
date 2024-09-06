
using Anomalia.Persistence.Data;
using Anomalia.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Persistence.Configuration;
public static class DataBaseConfig
{
    public static void AddDataBaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        string? connectionString = configuration.GetConnectionStringOrThrow("AnomaliaConnection");
        services.AddScoped<ApplicationDbContext>();
        services.AddDbContext<ApplicationDbContext>(
                 (sp, optionsBuilder) =>
                 {
                     ConvertDomainEventsToOutboxMessagesInterceptor? interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;

                     optionsBuilder.UseSqlServer(connectionString)
                         .AddInterceptors(interceptor);
                 });

      
    }

}

