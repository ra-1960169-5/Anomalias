using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Problemas.Commands.Delete;
internal sealed class DeleteProblemaCommandHandler(IProblemaRepository problemaRepository) : ICommandHandler<DeleteProblemaCommand>
{
    private readonly IProblemaRepository _problemaRepository = problemaRepository;

    public async Task<Result> Handle(DeleteProblemaCommand request, CancellationToken cancellationToken)
    {
        var problema = await _problemaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (problema is null) return DomainErrors.ProblemaErrors.NotFound;
        if (await _problemaRepository.PossuiAnomalias(request.Id)) return DomainErrors.ProblemaErrors.Delete;
        _problemaRepository.Remove(problema);
        if (await _problemaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
