using Anomalia.Application.Abstractions.Data;
using Anomalia.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext,IUnitOfWork
{
    public DbSet<Domain.Entities.Problema> Problemas { get; set; }
    public DbSet<Domain.Entities.Setor> Setores { get; set; }
    public DbSet<Domain.Entities.Cargo> Cargos { get; set; }
    public DbSet<Domain.Entities.Anomalia> Anomalias { get; set; }
    public DbSet<Domain.Entities.Funcionario> Funcionarios { get; set; }
    public DbSet<Domain.Entities.Anexo> Anexos { get; set; }
    public DbSet<Domain.Entities.Comentario> Comentarios { get; set; }
    public DbSet<Domain.Entities.Gestor> Gestores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
       return  await base.SaveChangesAsync(cancellationToken) > 0;     
    }
}
