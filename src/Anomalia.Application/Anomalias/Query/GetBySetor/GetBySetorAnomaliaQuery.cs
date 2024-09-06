using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anomalias.Query;

namespace Anomalias.Application.Anomalias.Query.GetBySetor;
public sealed record GetBySetorAnomaliaQuery(Guid UserId, DateTime DataInicial, DateTime DataFinal, int Status) : IQuery<IList<AnomaliaResponse>>;
