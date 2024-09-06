using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Anomalias.Query.GetAll;
public sealed record GetAllAnomaliaQuery(Guid UserId, DateTime DataInicial, DateTime DataFinal, int Status) : IQuery<IList<AnomaliaResponse>>;

