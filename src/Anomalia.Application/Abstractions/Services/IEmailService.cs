using Anomalias.Domain.Entities;

namespace Anomalias.Application.Abstractions.Services;
public interface IEmailService
{
    Task SendAnomaliaResgistredEmailAsync(Anomalia anomalia, CancellationToken cancellationToken);
    Task SendAnomaliaTerminatedEmailAsync(Anomalia anomalia, CancellationToken cancellationToken);
    Task SendComentarioResgistredEmailAsync(Comentario comentario, CancellationToken cancellationToken);
    Task SendRecoverPasswordEmailAsync(string email,string link);
}
