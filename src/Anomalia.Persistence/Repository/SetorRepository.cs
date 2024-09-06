using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.Repository;
public class SetorRepository(ApplicationDbContext context) : Repository<Setor,SetorId>(context), ISetorRepository
{
    public async Task<Setor?> FindByIdWithGestorAsync(SetorId? id,CancellationToken cancellationToken)
    {
        return await Dbset.Where(x=>x.Id==id).Include(x=>x.Gestor).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SetorId?> GetIdByDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        return await Dbset.AsNoTracking().Where(w => w.Descricao.ToUpper().Equals(description)).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
    }
}
