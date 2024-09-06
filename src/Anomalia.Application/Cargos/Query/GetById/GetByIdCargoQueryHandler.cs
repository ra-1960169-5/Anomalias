using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query;
using Anomalias.Application.Extensions;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Cargos.Query.GetById;
internal sealed class GetByIdCargoQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdCargoQuery, CargoResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<CargoResponse>> Handle(GetByIdCargoQuery request, CancellationToken cancellationToken)
    {
        var cargo = await _dbContext.Cargos.AsNoTracking().Where(w => w.Ativo == true && w.Id == request.Id).Select(cargo => cargo.ToResponse()).FirstOrDefaultAsync(cancellationToken);
        if (cargo is null) return Result.Failure<CargoResponse>(new Error("Cargo.NotFound", "cargo não encontrado!"));
        return Result.Success(cargo);
    }
}
