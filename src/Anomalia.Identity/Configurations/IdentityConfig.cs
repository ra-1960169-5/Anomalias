using Anomalia.Identity.Data;
using Anomalia.Identity.Extensions;
using Anomalia.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Identity.Configurations;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityDataBaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        string? connectionString = configuration.GetConnectionString("IdentityConnection");

        // config db
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        options.UseSqlServer(connectionString));

        return services;
    }
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {


        //config Identity
        services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<Role>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>(); ;



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

}