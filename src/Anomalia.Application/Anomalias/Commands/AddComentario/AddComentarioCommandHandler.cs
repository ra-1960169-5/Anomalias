using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Application.Anomalias.Commands.AddComentario;
internal sealed class AddComentarioCommandHandler(IAnomaliaRepository anomaliaRepository) : ICommandHandler<AddComentarioCommand>
{
    private readonly IAnomaliaRepository _anomaliaRepository = anomaliaRepository;
    public async Task<Result> Handle(AddComentarioCommand request, CancellationToken cancellationToken)
    {
        var anomalia = await _anomaliaRepository.GetByIdAsync(AnomaliaId.TryParse(request.AnomaliaId)!.Value, cancellationToken);
        if (anomalia is null) return DomainErrors.AnomaliaErrors.NotFound;
        var addComentarioResult = anomalia.AddComentario(request.Comentario, request.ComentaristaId, request.DataDoComentario);
        if (addComentarioResult.IsFailure) return Result.Failure(addComentarioResult.Errors);
        if (request.Anexo is not null && addComentarioResult.Value.AddAnexoComentario(request.Anexo.FileName, request.Anexo.ContentType, request.Anexo.Dados) is Result addAnexoComentarioResult && addAnexoComentarioResult.IsFailure)
            return Result.Failure(addAnexoComentarioResult.Errors);
        _anomaliaRepository.Update(anomalia);
        if (await _anomaliaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure)
            return Result.Failure(resultCommit.Errors);
        return Result.Success();
    }
}

