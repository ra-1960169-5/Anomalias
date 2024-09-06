using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Shared;
namespace Anomalias.Application.Users.Commands.RecoverPasswordUser;
internal sealed class RecoverPasswordUserCommandHandler(IIdentityService identityService, IEmailService emailService) : ICommandHandler<RecoverPasswordUserCommand>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(RecoverPasswordUserCommand request, CancellationToken cancellationToken)
    {
        var userCodeResult = await _identityService.GeneratePasswordResetTokenAsync(request.Email);
        if(userCodeResult.IsFailure) return Result.Failure(userCodeResult.Errors);
        await _emailService.SendRecoverPasswordEmailAsync(request.Email, userCodeResult.Value);
        return Result.Success();
    }
}
