using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Problemas.Commands.Create;
public sealed record CreateProblemaCommad(string Descricao) : ICommand;

