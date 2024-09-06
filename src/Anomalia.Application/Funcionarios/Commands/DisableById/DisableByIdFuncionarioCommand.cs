using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Funcionarios.Commands.DisableById;
public sealed record DisableByIdFuncionarioCommand(FuncionarioId Id) : ICommand;
