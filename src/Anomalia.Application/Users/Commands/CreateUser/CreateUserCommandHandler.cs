using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Domain.Errors;
using Anomalias.Domain.ValueObjects;
using Anomalias.Shared;

namespace Anomalias.Application.Users.Commands.CreateUser;
internal sealed class CreateUserCommandHandler(IIdentityService identityService) : ICommandHandler<CreateUserCommand, string>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        return await Result.Combine(
            Email.Create(request.Email),
            Result.Ensure(await _identityService.IsEmailUniqueAsync(request.Email), (e => e is true, DomainErrors.EmailErrors.EmailNotUnique)))
                  .Bind(async item => await _identityService.RegisterUserAsync(item.Item1.Value, request.Password))
                  .Tap(async Id => await _identityService.AddRoleToUserAsync(Id, "USUARIO"));
    }

}


