using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Repository;
public interface IAnexoRepository : IRepository<Domain.Entities.Anexo, AnexoId>
{
}
