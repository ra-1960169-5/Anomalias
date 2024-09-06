﻿using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anomalias.Query;
using Anomalias.Application.Extensions;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Query.GetBySetor;
internal sealed class GetBySetorAnomaliaQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetBySetorAnomaliaQuery, IList<AnomaliaResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IList<AnomaliaResponse>>> Handle(GetBySetorAnomaliaQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Funcionarios.AsNoTracking().Where(x => x.Id.Equals(new(request.UserId))).FirstOrDefaultAsync(cancellationToken);
        if (user is null) return Result.Failure<IList<AnomaliaResponse>>(DomainErrors.FuncionarioErrors.NotFound);
        var query = _dbContext.Anomalias.GetBySetorId(request.DataInicial, request.DataFinal, request.Status, user.SetorId);
        var response = await query.ToResponse().ToListAsync(cancellationToken);
        if (response is null) Result.Failure<IList<AnomaliaResponse>>(new Error("", ""));
        return response;
    }
}
