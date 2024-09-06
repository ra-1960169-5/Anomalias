using Anomalia.Application.Abstractions.Services;
using Anomalia.Application.Users.Query;
using Anomalia.Identity.Data;
using Anomalia.Identity.Errors;
using Anomalia.Identity.Model;
using Anomalia.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Identity.Services;
public class IdentityService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<Role> roleManager, ApplicationIdentityDbContext context) : IIdentityService
{
    public async  Task<Result> DeleteUserAsync(string userId)
    {
        var user =  await userManager.FindByIdAsync(userId);
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
        return Result.Success(new UserResponse(result.Id,result.UserName,result.Email));
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await context.Users.AnyAsync(s => s.Email == email);     
    }

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

    public async Task<Result<string>> RegisterUserAsync(string email, string password)
    {       
        IdentityUser user = new() { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded) return Result.Failure<string>(IdentityErrors.User.UserCreate);
        var userId = await GetIdUserByEmail(email);   
        return userId;
    }

    public async Task<Result> RegisterUserAsync(string id, string email, string password)
    {
      const string _USER_ROLE = "USUARIO";
      var user = new IdentityUser {Id=id, UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, password);   
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, _USER_ROLE);
            return Result.Success();
        }
        var errors = result.Errors.Select(s => s.Description).ToArray();
        string combindedString = string.Join(" ", errors);
        return Result.Failure(new Error("IdentityErrors", combindedString));
    }

    public async Task<Result> SignOutAsync()
    {
        await signInManager.SignOutAsync();
        return Result.Success();
    }
}
