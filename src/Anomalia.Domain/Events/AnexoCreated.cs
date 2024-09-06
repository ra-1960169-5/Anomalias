using Anomalias.Domain.Entities;
using Anomalias.Shared;

namespace Anomalias.Domain.Events;
public sealed record AnexoCreated(AnexoId Id) : IDomainEvent;


