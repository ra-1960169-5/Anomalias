using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Problemas.Commands.Update;
internal sealed class UpdateProblemaCommandHandler(IProblemaRepository problemaRepository) : ICommandHandler<UpdateProblemaCommand>
{
    private readonly IProblemaRepository _problemaRepository = problemaRepository;

    public async Task<Result> Handle(UpdateProblemaCommand request, CancellationToken cancellationToken)
    {
        var problema = await _problemaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (problema is null) return DomainErrors.ProblemaErrors.NotFound;
        problema.Update(request.Descricao.ToUpper());
        _problemaRepository.Update(problema);
        if (await _problemaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
