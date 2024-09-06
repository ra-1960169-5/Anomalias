using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Shared;

namespace Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
internal sealed class RegisterAnomaliaCommandHandler(IAnomaliaRepository anomaliaRepository) : ICommandHandler<RegisterAnomaliaCommand, string>
{
    private readonly IAnomaliaRepository _anomaliaRepository = anomaliaRepository;
    public async Task<Result<string>> Handle(RegisterAnomaliaCommand request, CancellationToken cancellationToken)
    {
        var anomalia = Domain.Entities.Anomalia.Create(
                        problemaId: request.ProblemaId,
                        setorId: request.SetorId,
                        restrita: request.Restrita,
                        dataDeAbertura: request.DataDeAbertura,
                        usuarioAberturaId: request.UsuarioAberturaId,
                        questionamento: request.Questionamento,
                        resultadoEsperado: request.ResultadoEsperado);
        if (anomalia.IsFailure) return Result.Failure<string>(anomalia.Errors);
        if (request.Anexo is not null && anomalia.Value.AddAnexoAnomalia(request.Anexo.FileName, request.Anexo.ContentType, request.Anexo.Dados) is Result addAnexoResult && addAnexoResult.IsFailure)
            return Result.Failure<string>(addAnexoResult.Errors);
        _anomaliaRepository.Add(anomalia.Value);
        if(await _anomaliaRepository.UnitOfWork.CommitAsync(cancellationToken) is Result resultCommit && resultCommit.IsFailure) return Result.Failure<string>(resultCommit.Errors);
        return Result.Success(anomalia.Value.Id.ToString());
    }
}


