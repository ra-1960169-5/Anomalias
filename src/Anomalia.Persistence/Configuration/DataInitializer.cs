using Anomalia.Application.Abstractions.Services;
using Anomalia.Application.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalia.Persistence.Configuration;
public static class DataInitializer
{
    public async static Task SeedDataAnomalia(this IServiceCollection services)
    {
        try
        {
            var provider = services.BuildServiceProvider();          
            await SeedFuncionario(provider);           
        }
        catch( Exception ex) {
            throw new Exception(ex.Message);
        }       
    }
  
    public async static Task SeedFuncionario(ServiceProvider provider, CancellationToken cancellationToken=default)
    {       
        var identityService = provider.GetRequiredService<IIdentityService>();
        var cargoRepository = provider.GetRequiredService<ICargoRepository>();
        var setorRepository = provider.GetRequiredService<ISetorRepository>();
        var funcionarioRepository = provider.GetRequiredService<IFuncionarioRepository>();

        const string EMAIL = "admin@anomalia.com.br";
        const string CARGO = "DIRETOR";
        const string SETOR = "TI";

        var user = await identityService.GetUserByEmail(EMAIL);
        if (user.IsFailure || await funcionarioRepository.ExistByIdAsync(user.Value.Id, cancellationToken)) return;       
        var cargo = await cargoRepository.GetIdByDescriptionAsync(CARGO,cancellationToken);
        var setor = await setorRepository.GetIdByDescriptionAsync(SETOR, cancellationToken);

        if (setor is not null && cargo is not null)
        { 
            var funcionario = Domain.Entities.Funcionario.Create(new(Guid.Parse(user.Value.Id)),user.Value.Nome!, user.Value.Email!, setor.Value.ToString(), cargo.Value.ToString(),false);
            if (funcionario.IsFailure) return;
                funcionarioRepository.Add(funcionario.Value);
          await funcionarioRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
        return;        
    }
}
