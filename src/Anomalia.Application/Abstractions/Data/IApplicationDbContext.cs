using Anomalias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Abstractions.Data;
public interface IApplicationDbContext
{
    public DbSet<Domain.Entities.Problema> Problemas { get; set; }
    public DbSet<Domain.Entities.Setor> Setores { get; set; }
    public DbSet<Domain.Entities.Cargo> Cargos { get; set; }
    public DbSet<Domain.Entities.Anomalia> Anomalias { get; set; }
    public DbSet<Domain.Entities.Funcionario> Funcionarios { get; set; }
    public DbSet<Domain.Entities.Anexo> Anexos { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
   // public DbSet<Gestor> Gestores { get; set; }
}
