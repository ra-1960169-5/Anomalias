using Microsoft.EntityFrameworkCore;
using Anomalias.Domain.Entities;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Infrastructure.Persistence.Data;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class CargoRepository(ApplicationDbContext context) : Repository<Cargo, CargoId>(context), ICargoRepository
{
    public async Task<CargoId?> GetIdByDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        return await Dbset.AsNoTracking().Where(w => w.Descricao.ToUpper().Equals(description)).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
    }
}
