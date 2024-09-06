using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Funcionarios.Commands.EnableById;
public sealed record EnableByIdFuncionarioCommand(FuncionarioId Id) : ICommand;
