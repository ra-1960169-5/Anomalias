using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;

namespace Anomalia.Persistence.Repository;
public class ComentarioRepository(ApplicationDbContext context) : Repository<Comentario, ComentarioId>(context), IComentarioRepository
{
}
