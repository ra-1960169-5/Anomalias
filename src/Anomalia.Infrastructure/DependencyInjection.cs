using Anomalias.Application.Abstractions.Services;
using Anomalias.Infrastructure.BackgroundJobs.Configuration;
using Anomalias.Infrastructure.Bus.Configuration;
using Anomalias.Infrastructure.DateProvider;
using Anomalias.Infrastructure.Email;
using Anomalias.Infrastructure.Identity.Configurations;
using Anomalias.Infrastructure.Persistence.Configuration;
using Anomalias.Infrastructure.UserContext;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Anomalias.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(
     this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();     
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserContext, UserService>();
        services.AddHttpContextAccessor();
        services.AddBus();     
        services.AddIdentity(configuration);
        services.AddPersistence(configuration);
        services.AddEmailConfiguration(configuration);     
        if (!env.IsEnvironment("Testing")) {
            services.AddBackgroundJobs();
            services.AddElasticApmForAspNetCore(new HttpDiagnosticsSubscriber(), new EfCoreDiagnosticsSubscriber());          
            services.SeedDataIdentity().Wait();
            services.SeedDataAnomalia().Wait();
        }
      

    }
}
