using Anomalia.Application.Abstractions.Data;
using Anomalia.Application.Repository;
using Anomalia.Persistence.BackgroundJobs.Configurations;
using Anomalia.Persistence.Configuration;
using Anomalia.Persistence.Data;
using Anomalia.Persistence.Interceptors;
using Anomalia.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataBaseConfiguration(configuration);
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddQuartzConfiguration();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<ICargoRepository, CargoRepository>();
        services.AddScoped<ISetorRepository, SetorRepository>();
        services.AddScoped<IAnexoRepository, AnexoRepository>();
        services.AddScoped<IAnomaliaRepository, AnomaliaRepository>();
        services.AddScoped<IProblemaRepository, ProblemaRepository>();
        services.AddScoped<IComentarioRepository, ComentarioRepository>();
        services.CreateDatabaseAnomalia();
      
        return services;
    }


   internal static IServiceCollection CreateDatabaseAnomalia(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var dataContext = provider.GetService<ApplicationDbContext>();
        dataContext?.Database.EnsureCreated();
        return services;
   }
}
