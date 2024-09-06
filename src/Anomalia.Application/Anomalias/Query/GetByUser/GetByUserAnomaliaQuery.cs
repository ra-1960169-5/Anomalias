using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anomalias.Query;

namespace Anomalias.Application.Anomalias.Query.GetByUser;
public sealed record class GetByUserAnomaliaQuery(Guid UserId, DateTime DataInicial, DateTime DataFinal, int Status) : IQuery<IList<AnomaliaResponse>>;
