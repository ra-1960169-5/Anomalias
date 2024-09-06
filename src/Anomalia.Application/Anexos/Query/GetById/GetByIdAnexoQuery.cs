using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Anexos.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Anexos.Query.GetById;
public sealed record GetByIdAnexoQuery(AnexoId Id) : IQuery<AnexoResponse>;

