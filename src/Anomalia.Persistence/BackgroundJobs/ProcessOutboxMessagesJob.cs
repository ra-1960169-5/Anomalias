using Anomalia.Application.Abstractions.Messaging;
using Anomalia.Persistence.Data;
using Anomalia.Infrastructure.OutBox;
using Anomalia.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Anomalia.Persistence.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IMediatorHandler mediator, ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMediatorHandler _mediator = mediator;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await GetOutboxMessages(context.CancellationToken);
        if (messages.Count==0) _logger.LogInformation("sem mensagens para processar!");

        foreach (OutboxMessage outboxMessage in messages)
        {
            try
            {
                IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerSettings);
                await _mediator.PublishEventAsync(domainEvent!, context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro enquanto processava a mensagem {MessageID}", outboxMessage.Id);
                outboxMessage.Error = ex.ToString();
            }

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }

    internal async Task<List<OutboxMessage>> GetOutboxMessages(CancellationToken cancellationToken)
    {

        return await _dbContext
                .Set<OutboxMessage>().OrderBy(o => o.OccurredOnUtc)
                .Where(m => m.ProcessedOnUtc == null)
                .Take(20)
                .ToListAsync(cancellationToken);
    }

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,        
    };
}