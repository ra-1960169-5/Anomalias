using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Repository;
public interface IAnomaliaRepository : IRepository<Domain.Entities.Anomalia, AnomaliaId>
{
    Task<Domain.Entities.Anomalia?> GetAnomaliaWithCommentsAndAttachmentById(AnomaliaId? id, CancellationToken cancellation);
}
