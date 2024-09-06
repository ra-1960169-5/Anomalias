using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anomalias.Infrastructure.Persistence.EntityConfiguration;
internal class AnexoMap : IEntityTypeConfiguration<Anexo>
{
    public void Configure(EntityTypeBuilder<Anexo> builder)
    {
        builder.ToTable(TableNames.Anexos);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedNever()
               .HasConversion(anexo => anexo.Value, value => new(value))
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Nome)
               .HasColumnType("varchar(200)")
               .HasColumnName("Nome")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(c => c.ContentType)
               .HasColumnType("varchar(100)")
               .HasColumnName("ContentType")
               .HasMaxLength(100)
               .IsRequired();


        builder.Property(c => c.Dados)
              .HasColumnType("varbinary(max)")
              .HasColumnName("Dados")            
              .IsRequired();


    }
}
