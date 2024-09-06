using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Infrastructure.Extensions;
using Anomalias.Infrastructure.Persistence.Data;
using Anomalias.Infrastructure.Persistence.Interceptors;
using Anomalias.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Persistence.Configuration;
public static class PersistenceConfiguration
{
    private static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("AnomaliaConnection");

        services.AddScoped<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(
                 (sp, optionsBuilder) =>
                 {
                     ConvertDomainEventsToOutboxMessagesInterceptor? interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;

                     optionsBuilder.UseSqlServer(connectionString)
                         .AddInterceptors(interceptor);
                 });

    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataBase(configuration);
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<ICargoRepository, CargoRepository>();
        services.AddScoped<ISetorRepository, SetorRepository>();
        services.AddScoped<IAnexoRepository, AnexoRepository>();
        services.AddScoped<IAnomaliaRepository, AnomaliaRepository>();
        services.AddScoped<IProblemaRepository, ProblemaRepository>();
        services.AddScoped<IComentarioRepository, ComentarioRepository>();
        return services;
    }



}

