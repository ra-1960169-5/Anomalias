using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Shared;

namespace Anomalias.Application.Setores.Commands.Create;
internal sealed class CreateSetorCommandHandler(ISetorRepository setorRepository) : ICommandHandler<CreateSetorCommad>
{
    private readonly ISetorRepository _setorRepository = setorRepository;
    public async Task<Result> Handle(CreateSetorCommad request, CancellationToken cancellationToken)
    {
        var setor = Domain.Entities.Setor.Create(request.Descricao.ToUpper());
        if (setor.IsFailure) return Result.Failure(setor.Errors);
        _setorRepository.Add(setor.Value);
        if (await _setorRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
