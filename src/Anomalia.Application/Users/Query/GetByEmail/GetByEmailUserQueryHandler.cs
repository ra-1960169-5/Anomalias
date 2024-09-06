using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Application.Users.Query;
using Anomalias.Shared;

namespace Anomalias.Application.Users.Query.GetByEmail;
internal sealed class GetByEmailUserQueryHandler(IIdentityService identityService) : IQueryHandler<GetByEmailUserQuery, UserResponse>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<UserResponse>> Handle(GetByEmailUserQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _identityService.GetUserByEmail(request.Email);
        if (userResult.IsFailure) return Result.Failure<UserResponse>(userResult.Errors);
        return userResult.Value;
    }
}
