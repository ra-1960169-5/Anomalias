using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Funcionarios.Commands.Update;
public record class UpdateFuncionarioCommand(FuncionarioId Id, string Nome, string SetorId, string CargoId, bool Ativo, bool Gestor) : ICommand;

