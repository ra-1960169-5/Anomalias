using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Cargos.Commands.UpdateCargo;
public sealed record UpdateCargoCommand(CargoId Id, string Descricao) : ICommand;
