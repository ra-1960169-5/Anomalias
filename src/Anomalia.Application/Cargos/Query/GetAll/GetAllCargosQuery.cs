using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query;

namespace Anomalias.Application.Cargos.Query.GetAll;
public sealed record GetAllCargosQuery() : IQuery<IList<CargoResponse>>;

