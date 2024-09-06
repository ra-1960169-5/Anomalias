using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.Repository;
public class FuncionarioRepository(ApplicationDbContext context) : Repository<Funcionario, FuncionarioId>(context), IFuncionarioRepository
{
    public async Task<bool> ExistByIdAsync(string id,CancellationToken cancellationToken)
    {
      if(Guid.TryParse(id, out Guid result))
       return await Dbset.AsNoTracking().Where(x => x.Id.Equals(new(result))).AnyAsync(cancellationToken);
      
       return false;
    }

    public async Task<Funcionario?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Dbset.Include(x=>x.Setor).ThenInclude(x=>x!.Gestor).Where(x => x.Id.Equals(new(id))).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Funcionario?> GetByIdFuncionarioWithSetorAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Dbset.Include(i=>i.Setor).Where(x=>x.Id.Value==id).FirstOrDefaultAsync(cancellationToken);
        
    }
}
