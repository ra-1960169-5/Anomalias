using Anomalia.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Bus;
public static class DependencyInjection
{
    public static IServiceCollection AddBusDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, InMemoryBus>();
        return services;
    }
}