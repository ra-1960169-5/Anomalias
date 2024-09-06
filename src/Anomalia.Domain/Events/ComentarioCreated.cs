using Anomalias.Domain.Entities;
using Anomalias.Shared;

namespace Anomalias.Domain.Events;
public sealed record ComentarioCreated(ComentarioId Id) : IDomainEvent;

