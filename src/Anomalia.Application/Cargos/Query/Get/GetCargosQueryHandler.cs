using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query;
using Anomalias.Application.Extensions;
using Anomalias.Shared;
using System.Linq.Expressions;

namespace Anomalias.Application.Cargos.Query.Get;
internal sealed class GetCargosQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetCargosQuery, IQueryable<CargoResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IQueryable<CargoResponse>>> Handle(GetCargosQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Cargo> cargosQuery = _dbContext.Cargos;
        await Task.Run(() =>
        {
            if (!string.IsNullOrWhiteSpace(request.Search)) cargosQuery = cargosQuery.Where(p => p.Descricao.Contains(request.Search));
            if (request.SortOrder?.ToLower() == "desc") cargosQuery = cargosQuery.OrderByDescending(GetSortProperty(request));
            else cargosQuery = cargosQuery.OrderBy(GetSortProperty(request));
        }, cancellationToken);
        var cargosResponsesQuery = cargosQuery.Select(cargo => cargo.ToResponse());
        return Result.Success(cargosResponsesQuery);

    }

    private static Expression<Func<Domain.Entities.Cargo, object>> GetSortProperty(GetCargosQuery request) =>
       request.SortColumn?.ToLower() switch
       {
           "descricao" => cargo => cargo.Descricao,
           _ => cargo => cargo.Id
       };
}
