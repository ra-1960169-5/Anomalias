using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Funcionarios.Commands.CreateFuncionario;
public sealed record CreateFuncionarioCommand(string Id, string Nome, string Email, string Setor, string Cargo, bool Gestor) : ICommand<string>;


