using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Problemas.Query;
using Anomalias.Shared;
using System.Linq.Expressions;

namespace Anomalias.Application.Problemas.Query.Get;
internal sealed class GetProblemaQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetProblemaQuery, IQueryable<ProblemaResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IQueryable<ProblemaResponse>>> Handle(GetProblemaQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Problema> problemaQuery = _dbContext.Problemas;
        await Task.Run(() =>
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
                problemaQuery = problemaQuery.Where(p => p.Descricao.Contains(request.Search));
            if (request.SortOrder?.ToLower() == "desc")
                problemaQuery = problemaQuery.OrderByDescending(GetSortProperty(request));
            else
                problemaQuery = problemaQuery.OrderBy(GetSortProperty(request));
        }, cancellationToken);
        var problemasResponsesQuery = problemaQuery.Select(problema => problema.ToResponse());
        return Result.Success(problemasResponsesQuery);
    }
    private static Expression<Func<Domain.Entities.Problema, object>> GetSortProperty(GetProblemaQuery request) =>
       request.SortColumn?.ToLower() switch
       {
           "descricao" => problema => problema.Descricao,
           _ => problema => problema.Id
       };
}

