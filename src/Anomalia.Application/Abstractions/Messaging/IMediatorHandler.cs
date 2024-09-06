using Anomalias.Shared;
using MediatR;

namespace Anomalias.Application.Abstractions.Messaging;
public interface IMediatorHandler
{
    Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken) where T : INotification;
    Task<Result<T>> SendCommandAsync<T>(T command, CancellationToken cancellationToken = default) where T : IRequest<Result<T>>;
    Task<T> SendCommandAsync<T>(IRequest<T> command, CancellationToken cancellationToken = default) where T : Result;
    Task<Result<object>> PublishIntegrationEventAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent;
}
