using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anomalias.Infrastructure.Persistence.Configuration;
public static class PersistenceDataInitializer
{
    private static ApplicationDbContext _context = null!;



    public async static Task SeedDataAnomalia(this IServiceCollection services)
    {
        try
        {
            var serviceProvider = services.BuildServiceProvider();     
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _context.Database.EnsureCreated();
            await SeedCargos();
            await SeedSetores();
            await SeedProblemas();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    internal async static Task SeedCargos(CancellationToken cancellationToken = default)
    {
        if (await _context.Cargos.AnyAsync(cancellationToken)) return;    
        await _context.Cargos.AddRangeAsync(Cargos(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }  
    internal async static Task SeedSetores(CancellationToken cancellationToken = default)
    {
        if (await _context.Setores.AnyAsync(cancellationToken)) return;
        await _context.Setores.AddRangeAsync(Setores(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    internal static async Task SeedProblemas(CancellationToken cancellationToken = default) {

        if (await _context.Problemas.AnyAsync(cancellationToken)) return;
        await _context.Problemas.AddRangeAsync(Problemas(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }


    internal static IList<Cargo> Cargos()
    {
        return [
             Cargo.Create(descricao: "DIRETOR").Value,
             Cargo.Create(descricao: "SUPERVISOR").Value,
             Cargo.Create(descricao: "ANALISTA").Value,
             Cargo.Create(descricao: "ASSISTENTE").Value,
             Cargo.Create(descricao: "AUXILIAR").Value
            ];
    }

    internal static IList<Setor> Setores() {

        return [Setor.Create(descricao: "TI").Value,
                Setor.Create(descricao: "DIRETORIA").Value,
                Setor.Create(descricao: "COMPRAS").Value,
                Setor.Create(descricao: "FINANCEIRO").Value,
                Setor.Create(descricao: "PATRIMÔNIO").Value,
                Setor.Create(descricao: "MARKETING").Value,
                Setor.Create(descricao: "RECURSOS HUMANOS").Value,
                Setor.Create(descricao: "CONTABILIDADE").Value,
                Setor.Create(descricao: "ALMOXARIFADO").Value,
                Setor.Create(descricao: "QUALIDADE").Value,
                Setor.Create(descricao: "ESTOQUE").Value,
                Setor.Create(descricao: "DISTRIBUIÇÃO").Value];
    }

    internal static IList<Problema> Problemas()
    {

        return [Problema.Create(descricao: "AMBIENTE").Value,
                Problema.Create(descricao: "MANUTENÇÃO").Value,
                Problema.Create(descricao: "SEGURANÇA").Value,
                Problema.Create(descricao: "PROCESSO").Value,
                Problema.Create(descricao: "QUALIDADE").Value,
                Problema.Create(descricao: "OUTROS").Value];
    }
}
