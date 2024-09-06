using Anomalias.Domain.Errors;
using Anomalias.Domain.Entities;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Shared;

namespace Anomalias.Application.Anomalias.Commands.EndAnomalia;
internal sealed class EndAnomaliaCommandHandler(IAnomaliaRepository anomaliaRepository) : ICommandHandler<EndAnomaliaCommand>
{
    private readonly IAnomaliaRepository _anomaliaRepository = anomaliaRepository;
    public async Task<Result> Handle(EndAnomaliaCommand request, CancellationToken cancellationToken)
    {
        var anomalia = await _anomaliaRepository.GetByIdAsync(AnomaliaId.TryParse(request.AnomalaiId)!.Value, cancellationToken);
        if (anomalia == null) return Result.Failure(DomainErrors.AnomaliaErrors.NotFound);
        var resultEndAnomaly = anomalia.EndAnomalia(request.DataDeEncerramento, request.UsuarioEncerramentoId, request.ConsideracoesFinais);
        if (resultEndAnomaly.IsFailure) return Result.Failure(resultEndAnomaly.Errors);
        _anomaliaRepository.Update(anomalia);
        if (await _anomaliaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure) 
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}
