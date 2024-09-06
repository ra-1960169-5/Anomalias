using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Setores.Commands.Delete;


public sealed record DeleteSetorCommand(SetorId Id) : ICommand;