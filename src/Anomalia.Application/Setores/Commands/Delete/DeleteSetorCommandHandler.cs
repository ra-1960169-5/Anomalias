using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Setores.Commands.Delete;
internal sealed class DeleteSetorCommandHandler(ISetorRepository setorRepository) : ICommandHandler<DeleteSetorCommand>
{
    private readonly ISetorRepository _setorRepository = setorRepository;

    public async Task<Result> Handle(DeleteSetorCommand request, CancellationToken cancellationToken)
    {
        var setor = await _setorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (setor is null) return DomainErrors.SetorErrors.NotFound;
        _setorRepository.Remove(setor);
        if (await _setorRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
