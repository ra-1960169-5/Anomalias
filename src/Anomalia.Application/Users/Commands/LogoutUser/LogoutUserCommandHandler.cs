using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Shared;

namespace Anomalias.Application.Users.Commands.LogoutUser;
internal class LogoutUserCommandHandler(IIdentityService identityService) : ICommandHandler<LogoutUserCommand>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _identityService.SignOutAsync();

        return Result.Success();
    }
}
