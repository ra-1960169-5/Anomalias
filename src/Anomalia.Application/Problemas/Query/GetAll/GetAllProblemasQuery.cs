using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Problemas.Query;

namespace Anomalias.Application.Problemas.Query.GetAll;
public sealed record GetAllProblemasQuery : IQuery<IList<ProblemaResponse>>;
