using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Repository;
public interface ICargoRepository : IRepository<Domain.Entities.Cargo, CargoId>
{
    Task<CargoId?> GetIdByDescriptionAsync(string description, CancellationToken cancellationToken);
}
