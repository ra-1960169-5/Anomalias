using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Repository;
public interface ISetorRepository : IRepository<Domain.Entities.Setor, SetorId>
{
    Task<Domain.Entities.Setor?> FindByIdWithGestorAsync(SetorId? id, CancellationToken cancellation);
    Task<SetorId?> GetIdByDescriptionAsync(string description, CancellationToken cancellationToken);
}
