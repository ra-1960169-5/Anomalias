using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anomalia.Persistence.EntityConfiguration;
internal class AnomaliaMap : IEntityTypeConfiguration<Domain.Entities.Anomalia>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Anomalia> builder)
    {
        builder.ToTable(TableNames.Anomalias);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever()
               .HasConversion(anomalia => anomalia.Value, value => new(value))
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Numero).HasColumnType("int")
               .UseIdentityColumn()
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(c => c.Restrita)
               .HasColumnType("bit")
               .HasColumnName("Restrita")
               .HasDefaultValue(false);

        builder.Property(c => c.DataDeAbertura)
               .HasColumnName("DataDeAbertura")
               .HasColumnType("DATETIME2");

        builder.Property(c => c.DataDeEncerramento)
               .HasColumnName("DataDeEncerramento")
               .HasColumnType("DATETIME2");

        builder.Property(c => c.ConsideracoesFinais)
               .HasColumnName("ConsideracoesFinais")
               .HasColumnType("varchar(500)");
       
        builder.Property(c => c.ResultadoEsperado)
               .HasColumnName("ResultadoEsperado")
               .HasColumnType("varchar(500)");
       
        builder.Property(c => c.Questionamento)
               .HasColumnName("Questionamento")
               .HasColumnType("varchar(500)");

        builder.Property(c => c.Status)
               .HasColumnType("int")
               .HasColumnName("Status");

        builder.Property(c => c.ProblemaId).ValueGeneratedNever()
               .HasConversion(problema => problema.Value, value => new(value))
               .HasColumnName("ProblemaId")
               .HasMaxLength(100);

        builder.Property(c => c.SetorId).ValueGeneratedNever()
               .HasConversion(setor => setor.Value, value => new(value))
               .HasColumnName("SetorId")
               .HasMaxLength(100);

        builder.Property(c => c.AberturaId).ValueGeneratedNever()
               .HasConversion(funcionarioId => funcionarioId.Value, value => new(value))
               .HasColumnName("AberturaId")
               .HasMaxLength(100);

        builder.Property(c => c.EncerramentoId).ValueGeneratedNever()              
               .HasColumnName("EncerramentoId")
               .HasMaxLength(100);

        builder.Property(c => c.AnexoAnomaliaId).ValueGeneratedNever()               
               .HasColumnName("AnexoAnomaliaId")
               .HasMaxLength(100);


        builder.HasOne(c => c.Setor);

        builder.HasOne(c => c.Problema);

        builder.HasMany(x => x.Comentarios).WithOne(x => x.Anomalia).HasForeignKey(x => x.AnomaliaId).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.AnexoAnomalia).WithOne().HasForeignKey<Domain.Entities.Anomalia>(x=>x.AnexoAnomaliaId);

    }
}
