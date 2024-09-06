using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Setores.Query;

namespace Anomalias.Application.Setores.Query.Get;
public sealed record GetSetorQuery(string? Search, string? SortColumn, string? SortOrder) : IQuery<IQueryable<SetorResponse>>;
