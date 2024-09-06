using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Setores.Query;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Setores.Query.GetById;
internal sealed class GetByIdSetorQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdSetorQuery, SetorResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<SetorResponse>> Handle(GetByIdSetorQuery request, CancellationToken cancellationToken)
    {
        // var setor = await _dbContext.Setores.Include(x => x.Gestor).ThenInclude(x => x!.Funcionario).AsNoTracking().Where(w => w.Ativo == true && w.Id == request.Id).Select(setor => setor.ToResponse()).FirstOrDefaultAsync(cancellationToken);
        var setor = await _dbContext.Setores.Include(x => x.Gestor).AsNoTracking().Where(w => w.Ativo == true && w.Id == request.Id).Select(setor => setor.ToResponse()).FirstOrDefaultAsync(cancellationToken);
        if (setor is null) return Result.Failure<SetorResponse>(DomainErrors.SetorErrors.NotFound);
        return Result.Success(setor);
    }
}

