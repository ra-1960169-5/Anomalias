using Anomalias.Application.Abstractions.Services;
using Anomalias.Application.Users.Query;
using Anomalias.Infrastructure.Identity.Data;
using Anomalias.Infrastructure.Identity.Enum;
using Anomalias.Infrastructure.Identity.Errors;
using Anomalias.Infrastructure.Identity.Model;
using Anomalias.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Anomalias.Infrastructure.Identity.Services;
public class IdentityService(UserManager<IdentityUser> userManager, 
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<Role> roleManager,
                             ApplicationIdentityDbContext context,
                             LinkGenerator linkGenerator,
                             IHttpContextAccessor httpContextAccessor
    
    ) : IIdentityService
{
    public async Task<Result> CreateRoleAsync(string name, int value)
    {
        var roleResult = await roleManager.CreateAsync(new Role(name, (EPermissions)value));
        if (roleResult.Succeeded) return Result.Success();
        return Result.Failure(IdentityErrors.Role.RoleCreate);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return Result.Failure(IdentityErrors.User.NotFound);
        await userManager.DeleteAsync(user);
        return Result.Success();
    }

    public async Task<Result<string>> GetIdUserByEmail(string email)
    {
        var result = await context.Users.Where(w => w.Email == email).Select(s => s.Id).SingleOrDefaultAsync();
        if (result is null) return Result.Failure<string>(IdentityErrors.User.NotFound);
        return Result.Success(result);
    }

    public async Task<Result<UserResponse>> GetUserByEmail(string email)
    {
        var result = await context.Users.Where(w => w.Email == email).SingleOrDefaultAsync();
        if (result is null) return Result.Failure<UserResponse>(IdentityErrors.User.NotFound);
        return Result.Success(new UserResponse(result.Id, result.UserName, result.Email));
    }

    public async Task<bool> IsEmailUniqueAsync(string email) =>
        !await context.Users.AnyAsync(s => s.Email == email);


    public async Task<bool> IsRoleExistAsync(string value) =>
         await roleManager.RoleExistsAsync(value);


    public async Task<Result> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return IdentityErrors.User.NotFound;
        var result = await signInManager.PasswordSignInAsync(user!, password, false, lockoutOnFailure: true);
        if (result.Succeeded) return Result.Success();
        if (result.IsLockedOut) return IdentityErrors.User.IsLockedOut;
        if (result.IsNotAllowed) return IdentityErrors.User.IsNotAllowed;
        return IdentityErrors.User.NotFound;
    }


    public async Task<Result<string>> RegisterUserAsync(string email, string password, string? name=null)
    {
        IdentityUser user = new() { UserName = name?.ToUpper()??email.ToUpper(), Email = email.ToUpper(), EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded) return user.Id;
        return Result.Failure<string>(IdentityErrors.User.UserCreate);
    }

 

    public async Task SignOutAsync() => await signInManager.SignOutAsync();


    public async Task<Result> AddRoleToUserAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return Result.Failure(IdentityErrors.User.NotFound);
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null) return Result.Failure(IdentityErrors.Role.NotFound);
        var result = await userManager.AddToRoleAsync(user, role.Name!);
        if (result.Succeeded) return Result.Success();
        return Result.Failure(IdentityErrors.Role.AddRoleToUser);
    }

    public async Task<Result<string>> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !(await userManager.IsEmailConfirmedAsync(user)))       
            return Result.Failure<string>(IdentityErrors.User.NotFound);      
        var userCode = await userManager.GeneratePasswordResetTokenAsync(user);
        userCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userCode));       
        var link  = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext!, "ResetPassword", "Account", new { code = userCode});      
        if (string.IsNullOrEmpty(link)) return Result.Failure<string>(IdentityErrors.User.NotFound);

         return Result.Success(link);
    }

    public async Task<Result> ResetPasswordAsync(string email, string code, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            Result.Failure(IdentityErrors.User.NotFound);
        var decode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));    
        var result = await userManager.ResetPasswordAsync(user!, decode, password);
        if (result.Succeeded) return Result.Success();

        return Result.Failure(new Error("ResetPassword","Falha ao cadastrar uma nova senha"));
    }
}
