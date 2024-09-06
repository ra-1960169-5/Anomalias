using Microsoft.AspNetCore.Http;

namespace Anomalias.Application.Abstractions.Services;
public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string UserName { get; }
 
}
