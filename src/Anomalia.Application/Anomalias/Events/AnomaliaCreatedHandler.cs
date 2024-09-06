using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Domain.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Events;
public sealed class AnomaliaCreatedHandler(IEmailService emailService, IApplicationDbContext dbContext) : IDomainEventHandler<AnomaliaCreated>
{

    private readonly IEmailService _emailService = emailService;
    private readonly IApplicationDbContext dbContext = dbContext;


    public async Task Handle(AnomaliaCreated notification, CancellationToken cancellationToken)
    {
        var anomalia = await dbContext
                            .Anomalias
                            .Include(x => x.FuncionarioAbertura)
                            .Include(x => x.Problema)
                            .Include(x => x.Setor)
                            .ThenInclude(x => x!.Funcionarios)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == notification.Id, cancellationToken);
        if (anomalia is null) return;

        await _emailService.SendAnomaliaResgistredEmailAsync(anomalia, cancellationToken);
    }
}
