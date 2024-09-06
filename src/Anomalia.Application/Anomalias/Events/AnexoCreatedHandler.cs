using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Domain.Events;

namespace Anomalias.Application.Anomalias.Events;
internal class AnexoCreatedHandler() : IDomainEventHandler<AnexoCreated>
{

    public async Task Handle(AnexoCreated notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
