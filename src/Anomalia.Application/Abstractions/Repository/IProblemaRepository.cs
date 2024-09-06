using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Repository;
public interface IProblemaRepository : IRepository<Domain.Entities.Problema, ProblemaId>
{
    Task<bool> PossuiAnomalias(ProblemaId id);
}
