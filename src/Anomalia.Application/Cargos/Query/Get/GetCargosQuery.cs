using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query;

namespace Anomalias.Application.Cargos.Query.Get;
public sealed record GetCargosQuery(string? Search, string? SortColumn, string? SortOrder) : IQuery<IQueryable<CargoResponse>>;

