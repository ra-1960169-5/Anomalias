using Anomalias.Infrastructure.OutBox;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;


namespace Anomalias.Infrastructure.Persistence.Interceptors;
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
     : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutBoxMessages(eventData.Context);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void InsertOutBoxMessages(DbContext context)
    {
               
        var outboxMessages = context.ChangeTracker
              .Entries<IAggregateRoot>()
              .Select(x => x.Entity)
              .SelectMany(aggregateRoot =>
              {
                  IReadOnlyCollection<IDomainEvent> domainEvents = aggregateRoot.GetDomainEvents();
                  aggregateRoot.ClearDomainEvents();
                  return domainEvents;
              })
              .Select(domainEvent => new OutboxMessage
              {
                  Id = Guid.NewGuid(),
                  OccurredOnUtc = DateTime.UtcNow,
                  Type = domainEvent.GetType().Name,
                  Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings)
              })
              .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);

    }

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
}

