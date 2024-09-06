using Anomalia.Application.Abstractions.Messaging;
using Anomalia.Shared;
using MediatR;

namespace Anomalia.Bus;
public class InMemoryBus(IMediator mediator) : IMediatorHandler
{
    private readonly IMediator _mediator = mediator;

    public async Task<Result<T>> SendCommandAsync<T>(T command, CancellationToken cancellationToken = default) where T : IRequest<Result<T>>
    {
        return await _mediator.Send(command, cancellationToken);
    }

    public async Task<T> SendCommandAsync<T>(IRequest<T> command, CancellationToken cancellationToken = default) where T : Result
    {
        return await _mediator.Send(command, cancellationToken);
    }

    public async Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken) where T : INotification
    {
        await _mediator.Publish(@event, cancellationToken);
    }

    public async Task<Result<object>> PublishIntegrationEventAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        return await _mediator.Send(@event, cancellationToken);
    }
}
