using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Setores.Commands.Update;
public sealed record UpdateSetorCommand(SetorId Id, string Descricao, string? GestorId = null) : ICommand;
