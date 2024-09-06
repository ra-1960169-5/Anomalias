using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Setores.Commands.Update;
internal sealed class UpdateSetorCommandHandler(ISetorRepository setorRepository, IFuncionarioRepository funcionarioRepository) : ICommandHandler<UpdateSetorCommand>
{
    private readonly ISetorRepository _setorRepository = setorRepository;
    private readonly IFuncionarioRepository _funcionarioRepository = funcionarioRepository;

    public async Task<Result> Handle(UpdateSetorCommand request, CancellationToken cancellationToken)
    {
        var setor = await _setorRepository.FindByIdWithGestorAsync(request.Id, cancellationToken);
        if (setor is null) return DomainErrors.SetorErrors.NotFound;
        FuncionarioId? gestorIdValue = FuncionarioId.TryParse(request.GestorId);
        var gestor = await _funcionarioRepository.GetByIdAsync(gestorIdValue!.Value, cancellationToken);
        if (setor.Update(request.Descricao.ToUpper(), gestor) is Result resultUpdate && resultUpdate.IsFailure) return Result.Failure(resultUpdate.Errors);
        _setorRepository.Update(setor);
        if (await _setorRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
