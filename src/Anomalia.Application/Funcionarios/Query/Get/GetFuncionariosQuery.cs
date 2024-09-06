using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Funcionarios.Query;


namespace Anomalias.Application.Funcionarios.Query.Get;
public sealed record GetFuncionariosQuery(string? Search, string? SortColumn, string? SortOrder) : IQuery<IQueryable<FuncionarioResponse>>;

