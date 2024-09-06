using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anexos.Query;
using Anomalias.Application.Extensions;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anexos.Query.GetById;
internal sealed class GetByIdAnexoQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdAnexoQuery, AnexoResponse>
{

    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<AnexoResponse>> Handle(GetByIdAnexoQuery request, CancellationToken cancellationToken)
    {
        var anexo = await _dbContext.Anexos.AsNoTracking().Where(anexo => anexo.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (anexo is null) return Result.Failure<AnexoResponse>(DomainErrors.AnexoErrors.NotFound);
        return Result.Success(anexo.ToResponse());

    }
}
