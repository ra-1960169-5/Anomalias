using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Setores.Query;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Setores.Query.GetAll;
internal sealed class GetAllSetoresQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllSetoresQuery, IList<SetorResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IList<SetorResponse>>> Handle(GetAllSetoresQuery request, CancellationToken cancellationToken)
    {
        var setores = await _dbContext.Setores.AsNoTracking().OrderBy(x => x.Descricao).Select(setor => setor.ToResponse()).ToListAsync(cancellationToken);

        return setores;
    }
}

