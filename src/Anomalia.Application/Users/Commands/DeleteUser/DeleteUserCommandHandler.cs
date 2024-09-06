using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Shared;

namespace Anomalias.Application.Users.Commands.DeleteUser;
internal sealed class DeleteUserCommandHandler(IIdentityService identityService) : ICommandHandler<DeleteUserCommand>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
     return await _identityService.DeleteUserAsync(request.Id);
    }
}
