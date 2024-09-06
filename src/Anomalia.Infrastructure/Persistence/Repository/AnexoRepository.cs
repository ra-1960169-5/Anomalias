using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Data;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class AnexoRepository(ApplicationDbContext context) : Repository<Anexo, AnexoId>(context), IAnexoRepository
{
}
