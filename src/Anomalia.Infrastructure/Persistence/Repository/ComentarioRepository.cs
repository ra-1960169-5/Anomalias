using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Data;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class ComentarioRepository(ApplicationDbContext context) : Repository<Comentario, ComentarioId>(context), IComentarioRepository
{
}
