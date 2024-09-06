using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Cargos.Query.GetById;
public sealed record GetByIdCargoQuery(CargoId Id) : IQuery<CargoResponse>;

