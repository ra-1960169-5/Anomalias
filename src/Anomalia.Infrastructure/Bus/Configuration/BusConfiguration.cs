using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Bus.Configuration;
public static class BusConfiguration
{
    internal static IServiceCollection AddBus(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, InMemoryBus>();
        return services;
    }

}
