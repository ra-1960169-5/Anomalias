using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Cargos.Commands.UpdateCargo;
internal sealed class UpdateCargoCommandHandler(ICargoRepository cargoRepository) : ICommandHandler<UpdateCargoCommand>
{
    private readonly ICargoRepository _cargoRepository = cargoRepository;

    public async Task<Result> Handle(UpdateCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _cargoRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cargo is null) return DomainErrors.CargoErrors.NotFound;
        cargo.Update(request.Descricao);
        _cargoRepository.Update(cargo);
        await _cargoRepository.UnitOfWork.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
