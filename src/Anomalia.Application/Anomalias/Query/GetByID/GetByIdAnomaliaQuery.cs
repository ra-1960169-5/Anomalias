using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anomalias.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Anomalias.Query.GetByID;
public sealed record GetByIdAnomaliaQuery(AnomaliaId Id) : IQuery<AnomaliaResponse>;


