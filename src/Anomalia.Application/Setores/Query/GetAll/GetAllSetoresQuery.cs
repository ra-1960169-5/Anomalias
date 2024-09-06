using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Setores.Query;

namespace Anomalias.Application.Setores.Query.GetAll;
public sealed record GetAllSetoresQuery : IQuery<IList<SetorResponse>>;

