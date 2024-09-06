using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Shared;


namespace Anomalias.Application.Users.Commands.LoginUser;
internal sealed class LoginUserCommandHandler(IIdentityService identityService) : ICommandHandler<LoginUserComand>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(LoginUserComand request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(request.Email, request.Password);
    }


}
