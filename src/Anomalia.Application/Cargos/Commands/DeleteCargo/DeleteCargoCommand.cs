using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Cargos.Commands.DeleteCargo;
public sealed record DeleteCargoCommand(CargoId Id) : ICommand;
