using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Funcionarios.Commands.Update;
internal sealed class UpdateFuncionarioCommandHandler(IFuncionarioRepository funcionarioRepository, ISetorRepository setorRepository) : ICommandHandler<UpdateFuncionarioCommand>
{

    private readonly IFuncionarioRepository _funcionarioRepository = funcionarioRepository;
    private readonly ISetorRepository _setorRepository = setorRepository;

    public async Task<Result> Handle(UpdateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var funcionario = await _funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);
        if (funcionario is null) return DomainErrors.FuncionarioErrors.NotFound;
        if (request.Gestor)
        {
            var setorRequest = await _setorRepository.FindByIdWithGestorAsync(SetorId.TryParse(request.SetorId), cancellationToken);
            if (setorRequest?.CheckSetorManager(funcionario.Id) is Result result && result.IsFailure) return result;
        }
        if (funcionario.Update(request.Nome, request.SetorId, request.CargoId, request.Ativo, request.Gestor) is Result updateFuncionario && updateFuncionario.IsFailure) return Result.Failure(updateFuncionario.Errors);
        _funcionarioRepository.Update(funcionario);
        if (await _funcionarioRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
        return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }

}
