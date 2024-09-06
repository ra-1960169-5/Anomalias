using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Cargos.Commands.CreateCargo;
public sealed record CreateCargoCommand(string Descricao) : ICommand<string>;