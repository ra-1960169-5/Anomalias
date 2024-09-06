using Anomalias.Application.Users.Query;
using Anomalias.Shared;

namespace Anomalias.Application.Abstractions.Services;
public interface IIdentityService
{
    Task<Result> LoginAsync(string email, string password);
    Task<Result<string>> RegisterUserAsync(string email, string password, string? name=null);
    Task<Result<string>> GetIdUserByEmail(string email);
    Task<Result<UserResponse>> GetUserByEmail(string email);
    Task<bool> IsEmailUniqueAsync(string email);
    Task SignOutAsync();
    Task<Result> DeleteUserAsync(string userId);
    Task<bool> IsRoleExistAsync(string value);
    Task<Result> CreateRoleAsync(string name, int value);
    Task<Result> AddRoleToUserAsync(string userId, string roleName);
    Task<Result<string>> GeneratePasswordResetTokenAsync(string email);
    Task<Result> ResetPasswordAsync(string email,string code, string password);
}
