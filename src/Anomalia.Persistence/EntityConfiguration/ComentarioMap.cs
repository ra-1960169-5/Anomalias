using Anomalia.Domain.Entities;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anomalia.Persistence.EntityConfiguration;
internal class ComentarioMap : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.ToTable(TableNames.Comentarios);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever()
               .HasConversion(comentarioId => comentarioId.Value, value => new(value))
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.DataDoComentario)
               .HasColumnType("DATETIME2")
               .HasColumnName("DataDoComentario")
               .IsRequired();

        builder.Property(c => c.Descricao)
               .HasColumnType("varchar")
               .HasColumnName("Descricao")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(c => c.AnomaliaId).ValueGeneratedNever()
               .HasConversion(anomaliaId => anomaliaId.Value, value => new(value))             
               .HasColumnName("AnomaliaId")
               .HasMaxLength(100);

        builder.Property(c => c.ComentadorId).ValueGeneratedNever()
               .HasConversion(funcionarioId => funcionarioId.Value, value => new(value))
               .HasColumnName("ComentadorId")
                .HasMaxLength(100);

        builder.Property(c => c.AnexoComentarioId).ValueGeneratedNever()          
               .HasColumnName("AnexoComentarioId")
               .HasMaxLength(100);



        builder.HasOne(x => x.AnexoComentario).WithOne().HasForeignKey<Comentario>(x => x.AnexoComentarioId);

    }
}
