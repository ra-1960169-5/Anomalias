using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Setores.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Setores.Query.GetById;
public sealed record GetByIdSetorQuery(SetorId Id) : IQuery<SetorResponse>;


