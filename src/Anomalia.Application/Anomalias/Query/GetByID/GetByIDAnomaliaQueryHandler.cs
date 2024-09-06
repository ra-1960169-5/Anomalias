using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anomalias.Query;
using Anomalias.Application.Extensions;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Query.GetByID;
internal sealed class GetByIDAnomaliaQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdAnomaliaQuery, AnomaliaResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<AnomaliaResponse>> Handle(GetByIdAnomaliaQuery request, CancellationToken cancellationToken)
    {

        var response = await _dbContext.Anomalias
                              .Include(x => x.FuncionarioAbertura)
                              .Include(x => x.FuncionarioEncerramento)
                              .Include(x => x.Problema)
                              .Include(x => x.Comentarios!.OrderBy(x => x.DataDoComentario)).ThenInclude(x => x.Comentador)
                              .Include(x => x.Setor).ThenInclude(x => x!.Gestor)
                              .AsNoTracking()
                              .GetById(request.Id)
                              .FirstOrDefaultAsync(cancellationToken);



        if (response is not null) return Result.Success(response.ToResponse());

        return Result.Failure<AnomaliaResponse>(DomainErrors.AnomaliaErrors.NotFound);



    }
}
