using Anomalia.Domain.Entities;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anomalia.Persistence.EntityConfiguration;
internal class GestorMap : IEntityTypeConfiguration<Gestor>
{
    public void Configure(EntityTypeBuilder<Gestor> builder)
    {
        builder.ToTable(TableNames.Gestores);

        builder.HasKey(c => new {c.FuncionarioId,c.SetorId});

        builder.Property(c => c.FuncionarioId)
                .ValueGeneratedNever()
                .HasConversion(funcionario => funcionario.Value, value => new(value))        
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(c => c.SetorId)
               .ValueGeneratedNever()
               .HasConversion(setor => setor.Value, value => new(value))      
               .HasMaxLength(100)
               .IsRequired();

     
        builder.HasOne(x => x.Funcionario).WithOne();

     
    }
}
