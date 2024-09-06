using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Problemas.Query;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Problemas.Query.GetAll;
internal sealed class GetAllProblemasQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllProblemasQuery, IList<ProblemaResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IList<ProblemaResponse>>> Handle(GetAllProblemasQuery request, CancellationToken cancellationToken)
    {
        var problemas = await _dbContext.Problemas.AsNoTracking().Where(w => w.Ativo == true).Select(problema => problema.ToResponse()).ToListAsync(cancellationToken);

        return problemas;
    }
}
