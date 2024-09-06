using Anomalias.Domain.Entities;
using Anomalias.Shared;

namespace Anomalias.Domain.Events;
public sealed record AnomaliaCreated(AnomaliaId Id) : IDomainEvent;
