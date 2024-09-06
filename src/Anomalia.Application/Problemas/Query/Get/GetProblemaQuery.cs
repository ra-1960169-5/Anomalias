using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Problemas.Query;

namespace Anomalias.Application.Problemas.Query.Get;
public sealed record GetProblemaQuery(string? Search, string? SortColumn, string? SortOrder) : IQuery<IQueryable<ProblemaResponse>>;
