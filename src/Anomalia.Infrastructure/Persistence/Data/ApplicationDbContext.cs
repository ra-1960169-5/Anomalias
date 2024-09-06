using Anomalias.Application.Abstractions.Data;
using Anomalias.Domain.Entities;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Infrastructure.Persistence.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    public DbSet<Problema> Problemas { get; set; }
    public DbSet<Setor> Setores { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Anomalia> Anomalias { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Anexo> Anexos { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.NoAction;
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }


    public async Task<Result> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await base.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch
        {
           return Result.Failure(new Error("Commit", "Falha ao realizar operação de banco de dados!"));
        }
    }
}
