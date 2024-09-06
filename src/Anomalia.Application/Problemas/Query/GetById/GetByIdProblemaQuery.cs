using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Problemas.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Problemas.Query.GetById;
public sealed record GetByIdProblemaQuery(ProblemaId Id) : IQuery<ProblemaResponse>;


