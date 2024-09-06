using Microsoft.AspNetCore.Http;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Application.Extensions;
namespace Anomalias.Infrastructure.UserContext;
internal sealed class UserService(IHttpContextAccessor contextAccessor) : IUserContext
{

    public bool IsAuthenticated => contextAccessor.HttpContext?.User.IsAuthenticated() ?? throw new NotImplementedException();

    public Guid UserId => contextAccessor.HttpContext?.User.GetUserId() ?? throw new ApplicationException("User context is unavailable");

    public string UserName => contextAccessor.HttpContext?.User.GetName() ?? throw new ApplicationException("User context is unavailable");


}
