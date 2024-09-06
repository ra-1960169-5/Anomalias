using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Funcionarios.Commands.DisableById;
internal sealed class DisableByIdFuncionarioCommandHandler(IFuncionarioRepository funcionarioRepository) : ICommandHandler<DisableByIdFuncionarioCommand>
{
    private readonly IFuncionarioRepository _funcionarioRepository = funcionarioRepository;

    public async Task<Result> Handle(DisableByIdFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var fucionario = await _funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);
        if (fucionario == null) return DomainErrors.FuncionarioErrors.NotFound;
        fucionario.DisableFuncionario();
        _funcionarioRepository.Update(fucionario);
        if (await _funcionarioRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
