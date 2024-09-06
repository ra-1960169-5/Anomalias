using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.Repository;
public class AnomaliaRepository(ApplicationDbContext context) : Repository<Domain.Entities.Anomalia, AnomaliaId>(context), IAnomaliaRepository
{
    public async Task<Domain.Entities.Anomalia?> GetAnomaliaWithCommentsAndAttachmentById(AnomaliaId? id, CancellationToken cancellationToken)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        var anomalia = await Dbset.Include(x => x.AnexoAnomalia).Include(x => x.Comentarios).ThenInclude(x=>x.AnexoComentario).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
      
        return anomalia;
    }
}
