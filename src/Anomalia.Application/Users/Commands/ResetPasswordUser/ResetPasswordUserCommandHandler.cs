using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Shared;

namespace Anomalias.Application.Users.Commands.ResetPasswordUser;

internal sealed class ResetPasswordUserCommandHandler(IIdentityService identityService) : ICommandHandler<ResetPasswordUserCommand>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
    {
      return  await _identityService.ResetPasswordAsync(request.Email, request.Code, request.Password);      
    }
}
