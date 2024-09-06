using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Problemas.Query;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Problemas.Query.GetById;
internal sealed class GetByIdProblemaQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdProblemaQuery, ProblemaResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<ProblemaResponse>> Handle(GetByIdProblemaQuery request, CancellationToken cancellationToken)
    {
        var problema = await _dbContext.Problemas.AsNoTracking().Where(w => w.Ativo == true && w.Id == request.Id).Select(problema => problema.ToResponse()).FirstOrDefaultAsync(cancellationToken);
        if (problema is null) return Result.Failure<ProblemaResponse>(new Error("Problema.NotFound", "problema não encontrado!"));
        return Result.Success(problema);
    }
}

