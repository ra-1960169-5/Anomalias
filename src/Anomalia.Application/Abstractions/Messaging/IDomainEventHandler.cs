using Anomalias.Shared;
using MediatR;

namespace Anomalias.Application.Abstractions.Messaging;
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
