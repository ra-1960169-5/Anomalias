using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Setores.Commands.Create;
public sealed record CreateSetorCommad(string Descricao) : ICommand;

