using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Funcionarios.Query;
using Anomalias.Shared;
using System.Linq.Expressions;

namespace Anomalias.Application.Funcionarios.Query.Get;
internal sealed class GetFuncionariosQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetFuncionariosQuery, IQueryable<FuncionarioResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IQueryable<FuncionarioResponse>>> Handle(GetFuncionariosQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Funcionario> funcionariosQuery = _dbContext.Funcionarios;
        await Task.Run(() =>
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
                funcionariosQuery = funcionariosQuery.Where(p => p.Nome.Contains(request.Search));
            if (request.SortOrder?.ToLower() == "desc")
                funcionariosQuery = funcionariosQuery.OrderByDescending(GetSortProperty(request));
            else
                funcionariosQuery = funcionariosQuery.OrderBy(GetSortProperty(request));
        }, cancellationToken);
        var funcionarioResponsesQuery = funcionariosQuery.Select(problema => problema.ToResponse());
        return Result.Success(funcionarioResponsesQuery);
    }



    private static Expression<Func<Domain.Entities.Funcionario, object>> GetSortProperty(GetFuncionariosQuery request) =>
       request.SortColumn?.ToLower() switch
       {
           "nome" => funcionario => funcionario.Nome,
           _ => funcionario => funcionario.Id
       };
}
