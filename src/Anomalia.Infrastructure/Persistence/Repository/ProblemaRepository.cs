using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class ProblemaRepository(ApplicationDbContext context) : Repository<Problema, ProblemaId>(context), IProblemaRepository
{
    public async Task<bool> PossuiAnomalias(ProblemaId id)
    {
        return await Dbset.Include(x=>x.Anomalias).Where(x=>x.Id ==id).Select(x=>x.Anomalias).AnyAsync();
    }
}
