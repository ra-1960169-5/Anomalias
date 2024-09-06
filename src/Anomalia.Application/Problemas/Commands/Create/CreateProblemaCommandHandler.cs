using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Problemas.Commands.Create;
internal sealed class CreateProblemaCommandHandler(IProblemaRepository problemaRepository) : ICommandHandler<CreateProblemaCommad>
{
    private readonly IProblemaRepository _problemaRepository = problemaRepository;

    public async Task<Result> Handle(CreateProblemaCommad request, CancellationToken cancellationToken)
    {
        var problema = Domain.Entities.Problema.Create(request.Descricao.ToUpper());
        if (problema.IsFailure) return Result.Failure(problema.Errors);
        _problemaRepository.Add(problema.Value);       
        if (await _problemaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
