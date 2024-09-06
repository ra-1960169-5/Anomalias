using Microsoft.EntityFrameworkCore;
using Anomalias.Domain.Entities;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Infrastructure.Persistence.Data;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class AnomaliaRepository(ApplicationDbContext context) : Repository<Anomalia, AnomaliaId>(context), IAnomaliaRepository
{
    public async Task<Anomalia?> GetAnomaliaWithCommentsAndAttachmentById(AnomaliaId? id, CancellationToken cancellationToken)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        var anomalia = await Dbset.Include(x => x.AnexoAnomalia).Include(x => x.Comentarios).ThenInclude(x => x.AnexoComentario).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        return anomalia;
    }
}
