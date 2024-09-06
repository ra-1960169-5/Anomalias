using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Cargos.Commands.DeleteCargo;
internal sealed class DeleteCargoCommandHandler(ICargoRepository cargoRepository) : ICommandHandler<DeleteCargoCommand>
{
    private readonly ICargoRepository _cargoRepository = cargoRepository;

    public async Task<Result> Handle(DeleteCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _cargoRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cargo is null) return DomainErrors.CargoErrors.NotFound;
        _cargoRepository.Remove(cargo);
        await _cargoRepository.UnitOfWork.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
