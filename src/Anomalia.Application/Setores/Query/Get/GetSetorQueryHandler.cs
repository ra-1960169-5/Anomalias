using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Setores.Query;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anomalias.Application.Setores.Query.Get;
internal sealed class GetSetorQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetSetorQuery, IQueryable<SetorResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IQueryable<SetorResponse>>> Handle(GetSetorQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Setor> setorQuery = _dbContext.Setores.Include(x => x.Gestor);//.ThenInclude(x => x!.Funcionario);
        await Task.Run(() =>
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
                setorQuery = setorQuery.Where(p => p.Descricao.Contains(request.Search));
            if (request.SortOrder?.ToLower() == "desc")
                setorQuery = setorQuery.OrderByDescending(GetSortProperty(request));
            else
                setorQuery = setorQuery.OrderBy(GetSortProperty(request));
        }, cancellationToken);
        var setoresResponsesQuery = setorQuery.Select(setor => setor.ToResponse());
        return Result.Success(setoresResponsesQuery);
    }
    private static Expression<Func<Domain.Entities.Setor, object>> GetSortProperty(GetSetorQuery request) =>
       request.SortColumn?.ToLower() switch
       {
           "descricao" => setor => setor.Descricao,
           _ => setor => setor.Id
       };
}

