using Anomalia.Domain.Entities;
using Anomalia.Application.Repository;
using Anomalia.Persistence.Data;

namespace Anomalia.Persistence.Repository;
public class ProblemaRepository(ApplicationDbContext context) : Repository<Problema, ProblemaId>(context), IProblemaRepository
{
}
