using Anomalia.Domain.Entities;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Anomalia.Persistence.EntityConfiguration;
internal class FuncionarioMap : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable(TableNames.Funcionarios);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Ativo)
               .HasColumnType("bit")
               .HasColumnName("Ativo")
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(c => c.GestorResponsavel)
               .HasColumnName("GestorResponsavel")
               .HasColumnType("bit")
               .HasDefaultValue(false);

        builder.Property(c => c.Id)
               .ValueGeneratedNever()
               .HasConversion(funcionarioId => funcionarioId.Value, value => new(value))
               .HasColumnName("Id")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Nome)
               .HasColumnType("varchar(100)")
               .HasColumnName("Nome")
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(c => c.Email)
               .HasColumnType("varchar(100)")
               .HasColumnName("Email")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.SetorId)
               .ValueGeneratedNever()
               .HasConversion(setorId => setorId.Value, value => new(value))
               .HasColumnName("SetorId")
               .HasMaxLength(100);
             
        builder.Property(c => c.CargoId)
               .ValueGeneratedNever()
               .HasConversion(cargoId => cargoId.Value, value => new(value))
               .HasColumnName("CargoId")
               .HasMaxLength(100);
           
        builder.HasOne(c => c.Cargo);   

        builder.HasMany(c => c.FuncionariosAbertura).WithOne(x => x.FuncionarioAbertura).HasForeignKey(x => x.AberturaId);

        builder.HasMany(c => c.FuncionariosEncerramento).WithOne(x => x.FuncionarioEncerramento).HasForeignKey(x => x.EncerramentoId).IsRequired(false);

        builder.HasMany(c => c.Comentarios).WithOne(x => x.Comentador).HasForeignKey(x => x.ComentadorId).OnDelete(DeleteBehavior.NoAction);

        
    }

}
