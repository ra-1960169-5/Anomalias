using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Shared;

namespace Anomalias.Application.Cargos.Commands.CreateCargo;
internal sealed class CreateCargoCommandHandler(ICargoRepository cargoRepository) : ICommandHandler<CreateCargoCommand, string>
{
    private readonly ICargoRepository _cargoRepository = cargoRepository;
    public async Task<Result<string>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = Domain.Entities.Cargo.Create(request.Descricao);
        if (cargo.IsFailure) return Result.Failure<string>(cargo.Errors);
        _cargoRepository.Add(cargo.Value);
        await _cargoRepository.UnitOfWork.CommitAsync(cancellationToken);
        return Result.Success(cargo.Value.Id.ToString());
    }
}
