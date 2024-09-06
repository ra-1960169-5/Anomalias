using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.Repository;
public class CargoRepository(ApplicationDbContext context) : Repository<Cargo,CargoId>(context), ICargoRepository
{  
    public async Task<CargoId?> GetIdByDescriptionAsync(string description, CancellationToken cancellationToken)
    {
      return  await Dbset.AsNoTracking().Where(w => w.Descricao.ToUpper().Equals(description)).Select(x=>x.Id).FirstOrDefaultAsync(cancellationToken);
    }
}
