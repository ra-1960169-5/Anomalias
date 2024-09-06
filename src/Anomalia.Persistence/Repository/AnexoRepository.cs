using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;

namespace Anomalia.Persistence.Repository;
public class AnexoRepository(ApplicationDbContext context) : Repository<Anexo, AnexoId>(context), IAnexoRepository
{
}
