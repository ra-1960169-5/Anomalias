using Anomalias.Application.Abstractions.Services;
using Anomalias.Infrastructure.Extensions;
using Anomalias.Infrastructure.Identity.Data;
using Anomalias.Infrastructure.Identity.Extensions;
using Anomalias.Infrastructure.Identity.Model;
using Anomalias.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Identity.Configurations;

public static class IdentityConfiguration
{
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<Role>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>(); 
        services.Configure<IdentityOptions>(options =>
        {
            //User
            options.User.AllowedUserNameCharacters = "ôóâãáàéêíabcdefghijklmnopqrstuvwxyzçÔÓÂÃÀÁÉÊÍABCDEFGHIJKLMNOPQRSTUVWXYZÇ0123456789-._@+~/ ";

            options.User.RequireUniqueEmail = true;


            //Lockout
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            //Password
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            //SignIn
            options.SignIn.RequireConfirmedAccount = true;



        });
        services.ConfigureApplicationCookie(options =>
        {
            //Cookie settings                    
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/acesso-negado";
            options.LogoutPath = "/sair";
            options.SlidingExpiration = true;
        });
        return services;
    }

    private static IServiceCollection AddIdentityDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("IdentityConnection");      
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        options.UseSqlServer(connectionString));
        return services;
    }



    internal static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityDataBase(configuration);
        services.AddIdentity();
        services.AddScoped<IIdentityService, IdentityService>();    
        return services;
    }
}