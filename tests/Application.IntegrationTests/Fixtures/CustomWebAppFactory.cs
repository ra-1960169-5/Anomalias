using Anomalias.Infrastructure.Identity.Configurations;
using Anomalias.Infrastructure.Identity.Data;
using Anomalias.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

namespace Application.IntegrationTests.Fixtures;
public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly MsSqlContainer _dbContainer;


    public CustomWebAppFactory()
    {
        _dbContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest").Build();
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing"); 
        var host = builder.Build();
        host.Start();
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(async services =>
        {
            string connectionStringAnomalia = _dbContainer.GetConnectionString().Replace("master","AnomaliaDBtest");
            string connectionStringIdentity = _dbContainer.GetConnectionString().Replace("master", "IdentityDBTest");
            services.RemoveDbContext<ApplicationDbContext>();
            services.RemoveDbContext<ApplicationIdentityDbContext>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionStringAnomalia));
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(connectionStringIdentity));
            services.EnsureDbCreated<ApplicationDbContext>();
            services.EnsureDbCreated<ApplicationIdentityDbContext>();
            await services.SeedDataIdentity();
        });
    }




}
