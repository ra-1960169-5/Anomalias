using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Cargos.Query.GetAll;
internal sealed class GetAllCargosQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllCargosQuery, IList<CargoResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IList<CargoResponse>>> Handle(GetAllCargosQuery request, CancellationToken cancellationToken)
    {
        var cargos = await _dbContext.Cargos.GetAll().ToResponse().ToListAsync(cancellationToken);
        if (cargos is null) return Result.Failure<IList<CargoResponse>>(DomainErrors.CargoErrors.NotFound);

        return cargos;

    }
}
