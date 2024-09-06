using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Events;

namespace Anomalias.Application.Anomalias.Events;
public class ComentarioCreatedHandler : IDomainEventHandler<ComentarioCreated>
{
    public async Task Handle(ComentarioCreated notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
